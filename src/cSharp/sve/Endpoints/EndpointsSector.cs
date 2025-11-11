using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;

namespace sve.Endpoints
{
    public static class SectorEndpoints
    {
        public static void MapSectorEndpoints(this WebApplication app)
        {
            var sector = app.MapGroup("/api/sectores");
            sector.WithTags("Sector");

            // Crear sector → SOLO Admin u Organizador
            sector.MapPost("/", (ISectorService sectorService, SectorCreateDto sector) =>
            {
                var validadorSector = new SectorValidator();
                var result = validadorSector.Validate(sector);
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

                var id = sectorService.AgregarSector(sector);
                return Results.Created($"/api/sectores/{id}", sector);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Obtener todos los sectores
            sector.MapGet("/", (ISectorService sectorService) =>
            {
                var sectores = sectorService.ObtenerTodo();
                return Results.Ok(sectores);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Obtener sector por ID
            sector.MapGet("/{sectorId}", (int sectorId, ISectorService sectorService) =>
            {
                var sectorDb = sectorService.ObtenerPorId(sectorId);
                if (sectorDb == null)
                    return Results.NotFound();
                return Results.Ok(sectorDb);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Actualizar sector
            sector.MapPut("/{sectorId}", (int sectorId, ISectorService sectorService, SectorUpdateDto sector) =>
            {
                var validadorSector = new ActualizarSector();
                var result = validadorSector.Validate(sector);
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

                var actualizado = sectorService.ActualizarSector(sectorId, sector);
                if (actualizado == 0)
                    return Results.NotFound();
                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Eliminar sector
            sector.MapDelete("/{sectorId}", (int sectorId, ISectorService sectorService) =>
            {
                var eliminado = sectorService.EliminarSector(sectorId);
                if (eliminado == 0)
                    return Results.NotFound();
                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });
        }
    }
}
