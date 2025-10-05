using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Services.Contracts;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // 🔹 Registro de usuario
        [HttpPost("auth/register")]
        public IActionResult Register([FromBody] RegisterDto usuario)
        {
            var usuarioId = _usuarioService.AgregarUsuario(usuario);
            return Ok(new { IdUsuario = usuarioId });
        }

        // 🔹 Login y devuelve token
        [HttpPost("auth/login")]
        public IActionResult Login([FromBody] LoginDto usuario)
        {
            var auth = _usuarioService.Login(usuario);
            return Ok(auth);
        }

        // 🔹 Refresh token
        [HttpPost("auth/refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenDto usuario)
        {
            var token = _usuarioService.RefreshToken(usuario);
            return Ok(new { Token = token });
        }

        // 🔹 Logout
        [HttpPost("auth/logout")]
        public IActionResult Logout([FromBody] RefreshTokenDto usuario)
        {
            _usuarioService.Logout(usuario.Token);
            return Ok();
        }

        // 🔹 Perfil del usuario autenticado
        [HttpGet("auth/me/{usuarioId}")]
        public IActionResult Me(int usuarioId)
        {
            var usuario = _usuarioService.GetProfile(usuarioId);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // 🔹 Lista de roles disponibles
        [HttpGet("auth/roles")]
        public IActionResult Roles()
        {
            var roles = _usuarioService.GetRoles();
            return Ok(roles);
        }

        // 🔹 Asignar/cambiar rol de un usuario
        [HttpPost("usuarios/{usuarioId}/roles")]
        public IActionResult AsignarRol(int usuarioId, [FromBody] UsuarioRolDto usuario)
        {
            var resultado = _usuarioService.AsignarRol(usuarioId, usuario);
            return Ok(resultado);
        }

        // 🔹 Obtener todos los usuarios
        [HttpGet("usuarios")]
        public IActionResult ObtenerTodo()
        {
            var usuarios = _usuarioService.ObtenerTodo();
            return Ok(usuarios);
        }

        // 🔹 Obtener usuario por ID
        [HttpGet("usuarios/{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var usuario = _usuarioService.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // 🔹 Actualizar usuario
        [HttpPut("usuarios/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] UsuarioUpdateDto usuario)
        {
            var result = _usuarioService.ActualizarUsuario(id, usuario);
            if (result == 0) return NotFound();
            return Ok();
        }

        // 🔹 Eliminar usuario
        [HttpDelete("usuarios/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var result = _usuarioService.EliminarUsuario(id);
            if (result == 0) return NotFound();
            return Ok();
        }
    }
}
