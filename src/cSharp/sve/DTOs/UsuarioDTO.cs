namespace sve.DTOs
{
    // Registro de usuario
    public class RegisterDto
    {
        public string Apodo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
    }

    // Login
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
    }

    // Respuesta de autenticación
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public UsuarioDto Usuario { get; set; } = null!;
    }

    // Refresh token
    public class RefreshTokenDto
    {
        public string Token { get; set; } = null!;
    }

    // Datos de usuario
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Apodo { get; set; } 
        public string Email { get; set; }
        public string Rol { get; set; } 
    }

    // Roles
    public class UsuarioRolDto
    {
        public string Rol { get; set; } 
    }

    // Actualización de usuario
    public class UsuarioUpdateDto
    {
        public string Apodo { get; set; }
        public string Email { get; set; } 
        public string? Contrasenia { get; set; } // Opcional cambiar contraseña
    }
}
