using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Model;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;


namespace WebStore.Controllers
{
    //[Route("Staff/[action]/{id?}")]
    [Authorize (Roles = Role.Administrators)]
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

        #region Details
        
        //[Route("~/Staff/info-{id}")]
        public IActionResult Details(int id)
        {
            var employer = _employersData.GetById(id);
            if (employer is null)
                return NotFound();
            return View(employer);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View("Edit", new EmployerViewModel());
        }

        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();

            var employer = _employersData.GetById(id);
            if (employer is null)
                return NotFound();

            return View(new EmployerViewModel
            {
                Id = employer.Id,
                Name = employer.Name,
                LastName = employer.LastName,
                Patronymic = employer.Patronymic,
                Age = employer.Age,
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _employersData.DeleteEmployer(id);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployerViewModel());

            var employer = _employersData.GetById((int)id);
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
            if (ModelState.IsValid)
            {
                var employer = new Employer
                {
                    Id = model.Id,
                    Name = model.Name,
                    LastName = model.LastName,
                    Patronymic = model.Patronymic,
                    Age = model.Age,
                };
                if (employer.Id == 0)
                    _employersData.AddEmployer(employer);
                else
                    _employersData.UpdateEmployer(employer);

                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }

        #endregion
    }
}
