using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
