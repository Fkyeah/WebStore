using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            return View(new CartOrderViewModel { Cart = _cartService.GetViewModel() });
        }
        public IActionResult Add(int id)
        {
            _cartService.Add(id);
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
            return RedirectToAction("Index", "Cart");
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        // Метод для оформления заказа
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = _cartService.GetViewModel(),
                    Order = OrderModel,
                });

            var order = await OrderService.CreateOrder(
                User.Identity!.Name,
                _cartService.GetViewModel(),
                OrderModel);

            _cartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int Id)
        {
            ViewBag.OrderId = Id;
            return View();
        }
    }
}
