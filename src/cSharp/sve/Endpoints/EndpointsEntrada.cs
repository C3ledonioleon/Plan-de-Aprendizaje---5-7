using sveCore.DTOs;
using sveCore.Models;
using sveCore.Services.IServices;

namespace sve.Endpoints
{
    public static class EntradaEndpoints
    {
        public static void MapEntradaEndpoints(this WebApplication app)
        {
            var entradas = app.MapGroup("/api/entradas");
            entradas.WithTags("Entradas");
            entradas.RequireAuthorization();

            // Obtener todas las entradas
            entradas.MapGet("/", (IEntradaService entradaService) =>
            {
                var lista = entradaService.ObtenerTodo();
                return Results.Ok(lista);
            });

            // Obtener entrada por ID
            entradas.MapGet("/{entradaId}", (IEntradaService entradaService, int entradaId) =>
            {
                var entrada = entradaService.ObtenerPorId(entradaId);
                if (entrada == null)
                    return Results.NotFound();
                return Results.Ok(entrada);
            });

            // Anular entrada
            entradas.MapPost("{entradaId}/anular", (IEntradaService entradaService, int entradaId) =>
            {
                return !entradaService.AnularEntrada(entradaId)
                    ? Results.NotFound($"No se encontró la entrada con ID {entradaId}")
                    : Results.Ok($"La entrada con ID {entradaId} fue anulada correctamente");
            });

            // Generar QR
            entradas.MapGet("/{entradaId}/qr", (int entradaId, IEntradaService entradaService, IQRService qrService, IOrdenService ordenService) =>
            {
                var entrada = entradaService.ObtenerPorId(entradaId);
                if (entrada == null)
                    return Results.NotFound($"No se encontró la entrada con ID {entradaId}.");

                var orden = ordenService.ObtenerPorId(entrada.IdOrden);
                if (orden == null)
                    return Results.BadRequest($"No se encontró la orden asociada a la entrada {entradaId}.");

                if (orden.Estado != EstadoOrden.Pagada)
                    return Results.BadRequest("Error: No se puede generar el código QR porque la orden no está pagada.");

                var contenido = $"http://localhost:5257/api/entradas/{entradaId}/qr"; // URL escaneable
                var qrBytes = qrService.GenerarQR(contenido);

                return Results.File(qrBytes, "image/png");
            });

            // Validar QR
            entradas.MapPost("/qr/validar", (QRValidacionDto qrDto, IEntradaService entradaService) =>
            {
                if (string.IsNullOrEmpty(qrDto.Contenido))
                    return Results.BadRequest("El contenido del QR es obligatorio.");

                var resultado = entradaService.ValidarQR(qrDto.Contenido);
                return Results.Ok(new { resultado });
            });
        }
    }
}
