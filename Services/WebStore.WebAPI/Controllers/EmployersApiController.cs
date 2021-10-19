using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain.Model;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/employers")]
    public class EmployersApiController : ControllerBase
    {
        private readonly IEmployersData _employersData;

        public EmployersApiController(IEmployersData employersData)
        {
            _employersData = employersData;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employers = _employersData.GetAllEmployers();
            return Ok(employers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employer = _employersData.GetById(id);
            if (employer is null)
                return NotFound();

            return Ok(employer);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Employer employer)
        {
            int id = _employersData.AddEmployer(employer);
            return CreatedAtAction(nameof(GetById), new { Id = id }, employer);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Employer employer)
        {
            _employersData.UpdateEmployer(employer);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _employersData.DeleteEmployer(id);
            return result ? Ok() : NotFound();
        }
    }
}
