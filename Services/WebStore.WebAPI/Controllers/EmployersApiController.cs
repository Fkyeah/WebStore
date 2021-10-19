using Microsoft.AspNetCore.Mvc;
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
    }
}
