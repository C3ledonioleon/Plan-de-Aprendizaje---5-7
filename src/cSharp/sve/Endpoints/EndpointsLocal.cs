using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;

namespace sve.Endpoints
{
    public static class LocalEndpoints
    {
        public static void MapLocalEndpoints(this WebApplication app)
        {
            var local = app.MapGroup("/api/locales");
            local.WithTags("Local");

            // Crear Local (solo ADMIN)
            local.MapPost("/", (ILocalService localService, LocalCreateDto local) =>
            {
                var validadorLocal = new LocalValidator();
                var result = validadorLocal.Validate(local);
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

                var id = localService.AgregarLocal(local);
                return Results.Created($"/api/local/{id}", local);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            // Obtener todos los locales (ADMIN + Organizador)
            local.MapGet("/", (ILocalService localService) =>
            {
                var locales = localService.ObtenerTodo();
                return Results.Ok(locales);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario" });

            // Obtener local por ID (ADMIN + Organizador)
            local.MapGet("/{localId}", (int localId, ILocalService localService) =>
            {
                var localEncontrado = localService.ObtenerPorId(localId);
                if (localEncontrado == null)
                    return Results.NotFound();
                return Results.Ok(localEncontrado);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario" });

            // Actualizar local (ADMIN + Organizador)
            local.MapPut("/{localId}", (int localId, ILocalService localService, LocalUpdateDto local) =>
            {
                var validadorLocal = new ActualizarLocal();
                var result = validadorLocal.Validate(local);
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

                var actualizado = localService.ActualizarLocal(localId, local);
                if (actualizado == 0)
                    return Results.NotFound();
                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Eliminar local (solo ADMIN)
            local.MapDelete("/{localId}", (int localId, ILocalService localService) =>
            {
                var eliminado = localService.EliminarLocal(localId);
                if (eliminado == 0)
                    return Results.NotFound();

                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });
        }
    }
}
