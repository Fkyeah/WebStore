using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
