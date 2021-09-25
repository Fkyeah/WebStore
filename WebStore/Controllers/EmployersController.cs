using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Model;

namespace WebStore.Controllers
{
    public class EmployersController : Controller
    {
        private readonly IEnumerable<Employer> _employers;
        public EmployersController()
        {
            _employers = TestData.EmployerList;
        }
        public IActionResult Index()
        {
            return View(_employers);
        }
        public IActionResult Details(int id)
        {
            return View(_employers.FirstOrDefault(t => t.Id == id));
        }
    }
}
