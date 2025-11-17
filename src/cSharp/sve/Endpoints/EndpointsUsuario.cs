using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;
using Microsoft.AspNetCore.Mvc;
using sveCore.Models;
namespace sve.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this WebApplication app)
        {
            var auth = app.MapGroup("/api/Auth");
            auth.WithTags("Auth");

            // Register → PUBLICO (sin permisos)
            auth.MapPost("Register", (IUsuarioService usuarioService, RegisterDto usuario) =>
            {
                var validadorRegistro = new RegisterDtoValidator();
                var result = validadorRegistro.Validate(usuario);

                if (!result.IsValid)
                {
                    var listaErrores = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return Results.ValidationProblem(listaErrores);
                }

                var id = usuarioService.Register(usuario);
                return Results.Created($"/api/usuarios/{id}", usuario);

            });

            // Login
            auth.MapPost("Login", (IUsuarioService usuarioService, LoginDto usuario) =>
            {
                var validadorLogin = new LoginDtoValidator();
                var result = validadorLogin.Validate(usuario);

                if (!result.IsValid)
                {
                    var listaErrores = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return Results.ValidationProblem(listaErrores);
                }

                var tokens = usuarioService.Login(usuario);
                return Results.Ok(tokens);
            });

            // Refresh Token
            auth.MapPost("Refresh", (IUsuarioService usuarioService, [FromBody] string refreshToken) =>
            {
                try
                {
                    var tokens = usuarioService.Refresh(refreshToken);
                    return Results.Ok(tokens);
                }
                catch (Exception ex)
                {
                    return Results.Json(new { error = ex.Message });
                }
            });

            // Logout
            auth.MapPost("Logout", [Authorize] (IUsuarioService usuarioService, HttpContext httpContext) =>
            {
                var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    usuarioService.Logout(email);
                    return Results.Ok(new { message = "Sesión cerrada correctamente." });
                }
                return Results.Unauthorized();
            });

            // Obtener datos del usuario actual
            auth.MapGet("/me", [Authorize] (HttpContext http) =>
            {
                var email = http.User.FindFirstValue(ClaimTypes.Email);
                var rol = http.User.FindFirstValue(ClaimTypes.Role);

                if (string.IsNullOrEmpty(email))
                    return Results.Unauthorized();

                return Results.Ok(new { email, rol });
            });
                // Obtener todas las tarifas
            auth.MapGet("/", (IUsuarioService usuarioService) =>
            {
                var usuario = usuarioService.ObtenerTodo();
                return Results.Ok(usuario);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Obtener roles disponibles
            auth.MapGet("Roles", (IUsuarioService usuarioService) =>
            {
                var roles = Enum.GetNames(typeof(RolUsuario));
                return Results.Ok(roles);
            });

            // Asignar rol a un usuario → SOLO ADMIN
            auth.MapPost("/usuarios/{usuarioId}/roles",
                [Authorize(Roles = "Administrador")]
                (int usuarioId, [FromBody] RolUsuario nuevoRol, IUsuarioService usuarioService) =>
                {
                    try
                    {
                        var usuario = usuarioService.GetById(usuarioId);
                        if (usuario == null)
                            return Results.NotFound(new { error = "Usuario no encontrado." });

                        usuarioService.UpdateRol(usuarioId, nuevoRol);
                        return Results.Ok(new { message = $"Rol asignado: {nuevoRol}" });
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new { error = ex.Message });
                    }
                });
        }
    }
}
