using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
