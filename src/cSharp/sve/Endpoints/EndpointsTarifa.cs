using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;

namespace sve.Endpoints
{
    public static class TarifaEndpoints
    {
        public static void MapTarifaEndpoints(this WebApplication app)
        {
            var tarifa = app.MapGroup("/api/tarifas");
            tarifa.WithTags("Tarifa");

            // Crear tarifa → Admin u Organizador
            tarifa.MapPost("/", (ITarifaService tarifaService, TarifaCreateDto tarifa) =>
            {
                var validadorTarifa = new TarifaValidator();
                var result = validadorTarifa.Validate(tarifa);
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

                var id = tarifaService.AgregarTarifa(tarifa);
                return Results.Created($"/api/tarifas/{id}", tarifa);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Obtener todas las tarifas
            tarifa.MapGet("/", (ITarifaService tarifaService) =>
            {
                var tarifas = tarifaService.ObtenerTodo();
                return Results.Ok(tarifas);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Obtener tarifa por ID
            tarifa.MapGet("/{tarifaId}", (int tarifaId, ITarifaService tarifaService) =>
            {
                var tarifaDb = tarifaService.ObtenerPorId(tarifaId);
                if (tarifaDb == null)
                    return Results.NotFound();
                return Results.Ok(tarifaDb);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Actualizar tarifa
            tarifa.MapPut("/{tarifaId}", (int tarifaId, ITarifaService tarifaService, TarifaUpdateDto tarifa) =>
            {
                var validadorTarifa = new ActualizarTarifa();
                var result = validadorTarifa.Validate(tarifa);
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

                var actualizado = tarifaService.ActualizarTarifa(tarifaId, tarifa);
                if (actualizado == 0)
                    return Results.NotFound();

                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Eliminar tarifa
            tarifa.MapDelete("/{tarifaId}", (int tarifaId, ITarifaService tarifaService) =>
            {
                var eliminado = tarifaService.EliminarTarifa(tarifaId);
                if (eliminado == 0)
                    return Results.NotFound();
                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });
        }
    }
}
