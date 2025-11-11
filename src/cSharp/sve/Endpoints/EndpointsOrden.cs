using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;

namespace sve.Endpoints
{
    public static class OrdenEndpoints
    {
        public static void MapOrdenEndpoints(this WebApplication app)
        {
            var orden = app.MapGroup("/api/ordenes");
            orden.WithTags("Orden");

            // Crear orden
            orden.MapPost("/", (OrdenCreateDto orden, IOrdenService ordenService) =>
            {
                var validadorOrden = new OrdenValidator();
                var result = validadorOrden.Validate(orden);
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

                var ordenId = ordenService.AgregarOrden(orden);
                return Results.Ok(new { IdOrden = ordenId });

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Usuario,Administrador" });

            // Obtener todas las órdenes
            orden.MapGet("/", (IOrdenService ordenService) =>
            {
                var ordenes = ordenService.ObtenerTodo();
                return Results.Ok(ordenes);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Molinete" });

            // Obtener orden por ID
            orden.MapGet("/{ordenId}", (int ordenId, IOrdenService ordenService) =>
            {
                var ordenDb = ordenService.ObtenerPorId(ordenId);
                if (ordenDb == null)
                    return Results.NotFound();
                return Results.Ok(ordenDb);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Molinete" });

            // Pagar orden
            orden.MapPost("/{ordenId}/pagar", (int ordenId, IOrdenService ordenService) =>
            {
                var result = ordenService.PagarOrden(ordenId);
                if (!result)
                    return Results.BadRequest("No se pudo procesar el pago o la orden no existe.");

                return Results.Ok(new { mensaje = "Orden pagada y entradas emitidas" });

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Usuario,Administrador" });

            // Cancelar orden
            orden.MapPost("/{ordenId}/cancelar", (int ordenId, IOrdenService ordenService) =>
            {
                var result = ordenService.CancelarOrden(ordenId);
                if (!result)
                    return Results.BadRequest("No se pudo cancelar la orden (puede que ya esté pagada o no exista).");

                return Results.Ok(new { mensaje = "Orden cancelada" });

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Usuario,Administrador" });
        }
    }
}
