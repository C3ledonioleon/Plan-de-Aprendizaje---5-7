namespace sve.Models;

public class Usuario
{
    public int IdUsuario {get; set;}
    public string Username {get; set;}
    public string Email {get; set; }
    public string Password {get; set; }
    public RolUsuario? Rol {get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiracion { get; set; }

    public Cliente? Cliente {get; set; }
}

public enum RolUsuario
{
    Administrador = 1,
    Empleado = 2,
    Cliente = 3,
    Soporte = 4
}