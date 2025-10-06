using sve.Models;

namespace sve.DTOs;

public class RegisterDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Contraseña { get; set; } = null!;
}

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Contraseña { get; set; } = null!;
}

public class TokenDto
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}