using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Route("Staff/[action]/{id?}")]
    public class EmployersController : Controller
    {
        private readonly IEmployersData _employersData;
        private readonly ILogger<EmployersController> _logger;

        public EmployersController(IEmployersData employersData, ILogger<EmployersController> logger)
        {
            _employersData = employersData;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(_employersData.GetAllEmployers());
        }
        [Route("~/Staff/info-{id}")]
        public IActionResult Details(int id)
        {
            var employer = _employersData.GetById(id);
            if (employer is null)
                return NotFound();
            return View(employer);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Edit(int id)
        {
            var employer = _employersData.GetById(id);
            if (employer is null)
                return NotFound();

            var model = new EmployerViewModel
            {
                Id = employer.Id,
                Name = employer.Name,
                LastName = employer.LastName,
                Patronymic = employer.Patronymic,
                Age = employer.Age,
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(EmployerViewModel model)
        {
            var employer = new Employer
            {
                Id = model.Id,
                Name = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Age = model.Age,
            };
            _employersData.UpdateEmployer(employer);

            return RedirectToAction(nameof(Index));
        }
    }
}
