using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("api/test-db")]
public class DbTestController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DbTestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var connectionString =
            _configuration.GetConnectionString("ProjectDatabase");

        using var connection = new SqlConnection(connectionString);

        var users = await connection.QueryAsync(@"
            SELECT TOP 5 
                Id, 
                UserName, 
                FullName 
            FROM Users
        ");

        return Ok(users);
    }
}
