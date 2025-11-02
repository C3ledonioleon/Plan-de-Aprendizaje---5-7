using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sveCore.DTOs;
using sveCore.Models;
using sveCore.Services.IServices;

using System.Security.Claims;
using static QRCoder.PayloadGenerator;

namespace sve.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public AuthController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost()]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        try
        {
            int id = _usuarioService.Register(dto);
            return Ok(new { message = "Usuario registrado correctamente.", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost()]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        try
        {
            var tokens = _usuarioService.Login(dto);
            return Ok(tokens);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpPost()]
    public IActionResult Refresh([FromBody] string refreshToken)
    {
        try
        {
            var tokens = _usuarioService.Refresh(refreshToken);
            return Ok(tokens);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost()]
    public IActionResult Logout()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        _usuarioService.Logout(email);
        return Ok(new { message = "Sesión cerrada correctamente." });
    }

    [Authorize]
    [HttpGet()]
    public IActionResult Me( )
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var rol = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new { email, rol });
    }

    [HttpGet()]
    public IActionResult Roles()
    {
        var roles = Enum.GetNames(typeof(RolUsuario));
        return Ok(roles);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost()]
    public IActionResult AsignarRol(int usuarioId, [FromBody] RolUsuario nuevoRol)
    {
        try
        {
            var usuario = _usuarioService.GetById(usuarioId);
            if (usuario == null)
                return NotFound(new { error = "Usuario no encontrado." });

            usuario.Rol = nuevoRol;
            _usuarioService.UpdateRol(usuarioId, nuevoRol);
            return Ok(new { message = $"Rol asignado: {nuevoRol}" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
