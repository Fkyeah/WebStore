using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #region Login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View( new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("Попытка входа пользователя {0}", model.UserName);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Введены некорректные данные");
                return View(model);
            }

            var resultLogin = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (resultLogin.Succeeded)
            {
                //if (Url.IsLocalUrl(model.ReturnUrl))
                //    return Redirect(model.ReturnUrl);
                //else
                //    return RedirectToAction("Index", "Home");
                var returnUrl = model.ReturnUrl ?? "/";
                _logger.LogInformation("Вход пользователя {0} выполнен успешно. Возврат на страницу {1}", model.UserName, returnUrl);
                return LocalRedirect(returnUrl);
            }

            var errorMessage = "Введены неверные данные";
            _logger.LogWarning("{0} для пользователя {1}. Возврат на страницу входа", errorMessage, model.UserName);
            ModelState.AddModelError("", errorMessage);
            return View(model);
        }
        #endregion

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Пользователь {0} вышел из системы", User.Identity!.Name);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            _logger.LogInformation("Отказано в доступе для пользователя {0} по адресу {1}", User.Identity!.Name, HttpContext.Request.Path);
            return View();
        }

        #region Register
        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            _logger.LogInformation("Попытка регистрации пользователя {0}", model.UserName);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Введены некорректные данные");
                return View(model);
            }
                
            var user = new User
            {
                UserName = model.UserName,
            };

            var registerResult = await _userManager.CreateAsync(user, model.Password);
            if (registerResult.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);
                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation("Пользователь {0} успешно вошел в систему", user.UserName);
                await _userManager.AddToRoleAsync(user, Role.Users);
                _logger.LogInformation("Пользователю {0} успешно выдана роль", user.UserName, Role.Users);
                _logger.LogInformation("Возврат на главную страницу");
                return RedirectToAction("Index", "Home");
            }
            
            foreach(var error in registerResult.Errors)
            {
                _logger.LogWarning("Ошибка при попытке регистрации: {0}", error.Description);
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        #endregion
    }
}
