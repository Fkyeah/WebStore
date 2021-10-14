using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class BlogSingleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
