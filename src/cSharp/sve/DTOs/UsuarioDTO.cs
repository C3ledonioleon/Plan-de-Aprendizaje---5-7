using sve.Models;

namespace sve.DTOs;

public class RegisterDto
{

    public string Username { get; set; } 
    public string Email { get; set; }
    public string Contraseña { get; set; } 
    public RolUsuario? Rol {get; set; } 
}

public class LoginDto
{
    public string Email { get; set; } 
    public string Contraseña { get; set; } 
}

public class TokenDto
{
    public string Token { get; set; } 
    public string RefreshToken { get; set; } 
}