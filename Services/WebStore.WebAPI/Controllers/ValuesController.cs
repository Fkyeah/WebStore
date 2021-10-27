using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebStore.Interfaces.WebAPI.Values)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Dictionary<int, string> _values;
        public ValuesController()
        {
            _values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value - {i}"))
            .ToDictionary(v => v.Id, v => v.Value);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_values.Values);


        [HttpGet]
        [Route("GetById/{Id}")]
        public IActionResult GetById(int id) => Ok(_values[id]);


        [HttpGet]
        [Route("Count")]
        public IActionResult Count() => Ok(_values.Count);

        [HttpPost("Add")]
        public IActionResult Add([FromBody] string value)
        {
            int id = _values.Max(v => v.Key) + 1;
            _values[id] = value;

            return CreatedAtRoute(nameof(GetById), new { Id = id });
        }

        [HttpPut("UpdateValue/{Id}")]
        public IActionResult Replace(int id, [FromBody] string value)
        {
            if (!_values.ContainsKey(id))
                return NotFound();

            _values[id] = value;
            return Ok();
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (!_values.ContainsKey(id))
                return NotFound();

            _values.Remove(id);
            return Ok();

        }
    }
}
