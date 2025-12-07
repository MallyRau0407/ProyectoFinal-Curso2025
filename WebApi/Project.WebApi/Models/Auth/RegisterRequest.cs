namespace Project.WebApi.Models.Auth;

public class RegisterRequest
{
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
