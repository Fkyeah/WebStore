using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.ViewModels.Users;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login() => View();
        public IActionResult Logout() => View();
        public IActionResult AccessDenied() => View();
        public IActionResult Register() => View(new RegisterUserViewModel);

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
