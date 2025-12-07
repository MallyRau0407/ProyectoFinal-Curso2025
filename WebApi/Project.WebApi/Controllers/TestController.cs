using Microsoft.AspNetCore.Mvc;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            message = "La API responde correctamente ✅",
            time = DateTime.UtcNow
        });
    }
}
