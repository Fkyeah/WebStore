using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Model;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;


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
            _logger.LogInformation("Получение информации о сотруднике с ID = {0}", id);
            var employer = _employersData.GetById(id);
            if (employer is null)
            {
                _logger.LogWarning("Пользователь с Id = {0} не найден", id);
                return NotFound();
            }
                
            return View(employer);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            _logger.LogInformation("Добавление нового сотрудника");
            return View("Edit", new EmployerViewModel());
        }

        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Попытка удаления пользователя с ID = {0}", id);
            if (id < 0)
            {
                _logger.LogWarning("В запросе передан некорректный ID = {0}", id);
                return BadRequest();
            }
                

            var employer = _employersData.GetById(id);
            if (employer is null)
            {
                _logger.LogWarning("Пользователь с ID = {0} не найден", id);
                return NotFound();
            }

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
            _logger.LogInformation("Пользователь с ID = {0} успешно удален", id);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        public IActionResult Edit(int? id)
        {
            _logger.LogInformation("Попытка редактирования пользователя с ID = {0}", id);
            if (id is null)
            {
                _logger.LogWarning("Не передан ID пользователя для редактирования");
                return View(new EmployerViewModel());
            }
                

            var employer = _employersData.GetById((int)id);
            if (employer is null)
            {
                _logger.LogWarning("Пользователь с ID = {0} не найден");
                return NotFound();
            }
                

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
                {
                    _logger.LogInformation("Создан новый пользователь {0} {1}", employer.Name, employer.LastName);
                    _employersData.AddEmployer(employer);
                } 
                else
                {
                    _employersData.UpdateEmployer(employer);
                    _logger.LogInformation("Редактирование пользователя с ID = {0} успешно выполнено", employer.Id);
                }
                    
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogWarning("Переданы некорректные данные");
                return View();
            }
        }

        #endregion
    }
}
