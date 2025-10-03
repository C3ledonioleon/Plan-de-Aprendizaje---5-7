using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Services;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // 🔹 Registro de usuario
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            var usuario = _usuarioService.Register(dto);
            return Ok(usuario);
        }

        // 🔹 Login y devuelve token
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var auth = _usuarioService.Login(dto);
            return Ok(auth);
        }

        // 🔹 Refresh token
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenDto dto)
        {
            var token = _usuarioService.RefreshToken(dto);
            return Ok(new { Token = token });
        }

        // 🔹 Logout
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] RefreshTokenDto dto)
        {
            _usuarioService.Logout(dto.Token);
            return Ok();
        }

        // 🔹 Perfil del usuario autenticado
        [HttpGet("me/{usuarioId}")]
        public IActionResult Me(int usuarioId)
        {
            var usuario = _usuarioService.GetProfile(usuarioId);
            return Ok(usuario);
        }

        // 🔹 Lista de roles disponibles
        [HttpGet("roles")]
        public IActionResult Roles()
        {
            var roles = _usuarioService.GetRoles();
            return Ok(roles);
        }

        // 🔹 Asignar/cambiar rol de un usuario
        [HttpPost("{usuarioId}/roles")]
        public IActionResult AsignarRol(int usuarioId, [FromBody] UsuarioRolDto dto)
        {
            var usuario = _usuarioService.AsignarRol(usuarioId, dto);
            return Ok(usuario);
        }
    }
}
