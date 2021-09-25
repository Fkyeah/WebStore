using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Model;

namespace WebStore.Controllers
{
    [Route("Staff/[action]/{id?}")]
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
        [Route("~/Staff/info-{id}")]
        public IActionResult Details(int id)
        {
            var employer = _employers.FirstOrDefault(t => t.Id == id);
            if (employer is null)
                return NotFound();
            return View(employer);
        }
    }
}
