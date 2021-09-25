using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
