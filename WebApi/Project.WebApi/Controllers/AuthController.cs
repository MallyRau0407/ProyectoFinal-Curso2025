using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using Project.WebApi.Models.Auth;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var cs = _configuration.GetConnectionString("ProjectDatabase");

        using var connection = new SqlConnection(cs);

        var sql = @"
            INSERT INTO Users (UserName, FullName)
            VALUES (@UserName, @FullName)";

        await connection.ExecuteAsync(sql, request);

        return Ok(new { message = "Usuario registrado correctamente ✅" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(RegisterRequest request)
    {
        var cs = _configuration.GetConnectionString("ProjectDatabase");

        using var connection = new SqlConnection(cs);

        var user = await connection.QueryFirstOrDefaultAsync(@"
            SELECT UserName, FullName
            FROM Users
            WHERE UserName = @UserName",
        new { request.UserName });

        if (user is null)
            return Unauthorized("Usuario no existe");

        return Ok(new
        {
            message = "Login correcto ✅",
            user
        });
    }

}
