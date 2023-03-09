using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebApiAddresses.Values)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _Logger;

        private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public ValuesController(ILogger<ValuesController> Logger) => _Logger = Logger;

        [HttpGet]
        public IActionResult Get() => Ok(_Values.Values);

        [HttpGet("{Id}")]
        public IActionResult GetById(int id)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();

            return Ok(_Values[id]);
        }

        [HttpGet("count")]
        public int Count() => _Values.Count;

        [HttpPost]
        [HttpPost("add")]
        public IActionResult Add([FromBody] string Value)
        {
            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
            _Values[id] = Value;

            return CreatedAtAction(nameof(GetById), new { id });
        }

        [HttpPut("{Id}")]
        public IActionResult Replace(int Id, [FromBody] string Value)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values[Id] = Value;

            return Ok();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values.Remove(Id);

            return Ok();
        }
    }
}
