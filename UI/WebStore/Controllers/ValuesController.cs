using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    public class ValuesController : Controller
    {
        private readonly IValueClient _valuesService;

        public ValuesController(IValueClient valuesService)
        {
            _valuesService = valuesService;
        }
        public IActionResult Index()
        {
            var values = _valuesService.GetAll();
            return View(values);
        }
    }
}
