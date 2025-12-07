using Microsoft.AspNetCore.Mvc;

namespace Project.WebApi.Controllers
{
    [ApiController]
    [Route("api/penalties")]
    public class PenaltiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var penalties = new[]
            {
                new { Id = 1, Amount = 50, Reason = "Retraso" },
                new { Id = 2, Amount = 100, Reason = "Daño" }
            };

            return Ok(penalties);
        }
    }
}
