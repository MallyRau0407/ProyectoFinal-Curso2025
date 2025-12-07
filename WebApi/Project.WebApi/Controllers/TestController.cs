using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Authorization;

namespace Project.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly string _connectionString;

    public TestController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' no está configurada."
            );
    }
    
    [HttpGet("secure")]
    public IActionResult SecureEndpoint()
    {
        return Ok(new
        {
            message = "Acceso autorizado con JWT ✅",
            user = User.Identity?.Name
        });
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            message = "La API responde correctamente ✅",
            time = DateTime.UtcNow
        });
    }

    [HttpGet("db-check")]
    public async Task<IActionResult> DbCheck()
    {
        using var conn = new SqlConnection(_connectionString);
        var dbName = await conn.ExecuteScalarAsync<string>("SELECT DB_NAME()");
        return Ok(new
        {
            connectedDatabase = dbName
        });
    }
}
