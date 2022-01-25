using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Возврат на страницу корзины");
            return View(new CartOrderViewModel { Cart = _cartService.GetViewModel() });
        }
        public IActionResult Add(int id)
        {
            _cartService.Add(id);
            _logger.LogInformation("Товар с ID = {0} добавлен в корзину", id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Decrement(int id)
        {
            _cartService.Decrement(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            _cartService.Remove(id);
            _logger.LogInformation("Товар с ID = {0} убран из корзины", id);
            return RedirectToAction("Index", "Cart");
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        // Метод для оформления заказа
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            _logger.LogInformation("Попытка оформления заказа для пользователя {0}", User.Identity!.Name);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("При оформлении заказа введены некоррктные данные");
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = _cartService.GetViewModel(),
                    Order = OrderModel,
                });
            }

            var order = await OrderService.CreateOrder(
                User.Identity!.Name,
                _cartService.GetViewModel(),
                OrderModel);
            _logger.LogInformation("Заказ для пользователя {0} успешно оформлен", User.Identity!.Name);
            _cartService.Clear();
            _logger.LogInformation("Id заказа = {0}", order.Id);
            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int Id)
        {
            ViewBag.OrderId = Id;
            return View();
        }

        #region WebAPI

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddAPI(int id)
        {
            _cartService.Add(id);
            _logger.LogInformation("Товар с ID = {0} добавлен в корзину", id);
            return Json(new { id, message = $"Товар с ID = {id} добавлен в корзину" });
        }
        public IActionResult DecrementAPI(int id)
        {
            _cartService.Decrement(id);
            return Json(new { id, message = $"Товар с ID = {id} уменьшен на единицу" });
        }
        public IActionResult RemoveAPI(int id)
        {
            _cartService.Remove(id);
            _logger.LogInformation("Товар с ID = {0} убран из корзины", id);
            return Json(new { id, message = $"Товар с ID = {id} убран из корзины" });
        }

        #endregion
    }
}
