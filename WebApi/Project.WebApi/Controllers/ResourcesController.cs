using Microsoft.AspNetCore.Mvc;

namespace Project.WebApi.Controllers
{
    [ApiController]
    [Route("api/resources")]
    public class ResourcesController : ControllerBase
    {
        // ✅ GET
        [HttpGet]
        public IActionResult GetAll()
        {
            var resources = new[]
            {
                new { Id = 1, Name = "Libro de prueba", Type = "Libro" },
                new { Id = 2, Name = "Laptop", Type = "Equipo" }
            };

            return Ok(resources);
        }

        // ✅ POST
        [HttpPost]
        public IActionResult Create([FromBody] CreateResourceRequest request)
        {
            // Simulamos que se guardó correctamente
            var resource = new
            {
                Id = 3,
                Name = request.Name,
                Type = request.Type
            };

            return Ok(resource);
        }
    }

    // ✅ DTO simple (puede ir en el mismo archivo por ahora)
    public class CreateResourceRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
