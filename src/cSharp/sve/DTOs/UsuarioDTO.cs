namespace sve.DTOs
{
    // 🔹 DTO para registrar usuario
    public class RegisterDto
    {
        public string Apodo { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
    }

    // 🔹 DTO para login
    public class LoginDto
    {
        public string Email { get; set; }
        public string Contrasenia { get; set; }
    }

    // 🔹 DTO para refresh token
    public class RefreshTokenDto
    {
        public string Token { get; set; }
    }

    // 🔹 DTO para devolver info básica del usuario (perfil)
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Apodo { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    // 🔹 DTO para cambio/asignación de roles
    public class UsuarioRolDto
    {
        public string Rol { get; set; }
    }

    // 🔹 DTO para devolver login exitoso
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UsuarioDto Usuario { get; set; }
    }
}
