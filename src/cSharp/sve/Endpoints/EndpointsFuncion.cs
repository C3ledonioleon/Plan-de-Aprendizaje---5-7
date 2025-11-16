using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;

namespace sve.Endpoints
{
    public static class FuncionEndpoints
    {
        public static void MapFuncionEndpoints(this WebApplication app)
        {
            var funcion = app.MapGroup("/api/funciones");
            funcion.WithTags("Funcion");
            funcion.RequireAuthorization();

            // Crear función
            funcion.MapPost("/", (IFuncionService funcionService, FuncionCreateDto funcion) =>
            {
                var validadorFuncion = new FuncionValidator();
                var result = validadorFuncion.Validate(funcion);

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

                var id = funcionService.AgregarFuncion(funcion);
                return Results.Created($"/api/funciones/{id}", funcion);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Obtener todas las funciones
            funcion.MapGet("/", (IFuncionService funcionService) =>
            {
                var funciones = funcionService.ObtenerTodo();
                if (funciones == null)
                    return Results.NotFound();

                return Results.Ok(funciones);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Obtener función por ID
            funcion.MapGet("/{funcionId}", (int funcionId, IFuncionService funcionService) =>
            {
                var funcionEncontrada = funcionService.ObtenerPorId(funcionId);
                if (funcionEncontrada == null)
                    return Results.NotFound();

                return Results.Ok(funcionEncontrada);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Actualizar función
            funcion.MapPut("/{funcionId}", (int funcionId, IFuncionService funcionService, FuncionUpdateDto funcion) =>
            {
                var validadorFuncion = new ActualizarFuncion();
                var result = validadorFuncion.Validate(funcion);

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

                var actualizado = funcionService.ActualizarFuncion(funcionId, funcion);
                if (actualizado == 0)
                    return Results.NotFound();

                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Eliminar función
            funcion.MapDelete("/{funcionId}", (int funcionId, IFuncionService funcionService) =>
            {
                var eliminado = funcionService.EliminarFuncion(funcionId);
                if (eliminado == 0)
                    return Results.NotFound();

                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            // Cancelar función
            funcion.MapPost("/{funcionId}/cancelar", (int funcionId, IFuncionService funcionService) =>
            {
                var actualizado = funcionService.CancelarFuncion(funcionId);
                if (actualizado == 0)
                    return Results.NotFound();

                return Results.Ok(new { mensaje = "Función cancelada correctamente" });

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });
        }
    }
}
