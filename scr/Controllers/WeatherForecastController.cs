using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new()
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<string> Get(int index)
        {
            return Summaries;
        }

        [HttpGet("{index}")]
        public string GetByIndex(int index)
        {
            if (index < 0 || index >= Summaries.Count) return "Такой индекс не существует";
            return Summaries[index];
        }

        [HttpGet("find-by-name/{name}")]
        public IActionResult GetByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest("Имя не может быть пустым");

            int count = 0;
            string ints = "";

            for (int i = 0; i < Summaries.Count; i++) {
                if (Summaries[i] == name) { ints += $" {i};"; count++; }
            }
            if (count == 0) return NotFound("Такого имени не существует");
            return Ok($"{count} записей {name} найдено на позициях:{ints}");
        }

        [HttpGet("get-by-sorted/{sortStrategy}")]
        public IActionResult GetWithSorted(int? sortStrategy)
        {
            switch (sortStrategy)
            {
                case -1: 
                    return Ok(Summaries.OrderBy(x => x));
                case 0: 
                    return Ok(Summaries);
                case 1: 
                    return Ok(Summaries.OrderByDescending(x => x));
                default:
                    return BadRequest("Некорректное значение параметра sortStrategy");
            }
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            Summaries.Add(name);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(int index, string name)
        {
            if (index < 0 || index >= Summaries.Count) return BadRequest("Такой индекс не найден/ не существует!");
            if (string.IsNullOrEmpty(name)) return BadRequest("Имя не может быть пустым");

            Summaries[index] = name;
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= Summaries.Count) return BadRequest("Такой индекс не найден/ не существует!");

            Summaries.RemoveAt(index);
            return Ok();
        }
    }
}