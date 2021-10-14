using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Orders([FromServices] IOrderService OrderService)
        {
            var orders = await OrderService.GetUserOrders(User.Identity!.Name);

            return View(orders.Select(order => new UserOrderViewModel
            {
                Id = order.Id,
                Address = order.Address,
                Phone = order.Phone,
                Description = order.Description,
                TotalPrice = order.TotalPrice,
                Date = order.Date,
            }));
        }
    }
}
