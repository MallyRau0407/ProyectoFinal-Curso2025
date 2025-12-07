using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Dapper;

namespace Project.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly string _connectionString;

    public UsersController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string no configurada");
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        using var connection = new SqlConnection(_connectionString);

        var users = await connection.QueryAsync("""
            SELECT Id, UserName, FullName, Role, IsActive
            FROM Users
        """);

        return Ok(users);
    }
}
