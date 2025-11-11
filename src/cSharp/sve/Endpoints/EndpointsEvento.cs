using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using sveServicio.Validation;

namespace sve.Endpoints
{
    public static class EventoEndpoints
    {
        public static void MapEventoEndpoints(this WebApplication app)
        {
            var evento = app.MapGroup("/api/eventos");
            evento.WithTags("Evento");
            evento.RequireAuthorization();

            // Crear evento
            evento.MapPost("/", (IEventoService eventoService, EventoCreateDto evento) =>
            {
                var validadorEvento = new EventoValidator();
                var result = validadorEvento.Validate(evento);

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

                var id = eventoService.AgregarEvento(evento);
                return Results.Created($"/api/eventos/{id}", evento);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Obtener todos los eventos
            evento.MapGet("/", (IEventoService eventoService) =>
            {
                var lista = eventoService.ObtenerTodo();
                return Results.Ok(lista);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario" });

            // Obtener evento por ID
            evento.MapGet("/{eventoId}", (int eventoId, IEventoService eventoService) =>
            {
                var eventoDb = eventoService.ObtenerPorId(eventoId);
                if (eventoDb == null) return Results.NotFound();
                return Results.Ok(eventoDb);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador,Usuario,Molinete" });

            // Actualizar evento
            evento.MapPut("/{eventoId}", (int eventoId, IEventoService eventoService, EventoUpdateDto evento) =>
            {
                var validadorEvento = new ActualizarEvento();
                var result = validadorEvento.Validate(evento);

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

                var actualizado = eventoService.ActualizarEvento(eventoId, evento);
                if (actualizado == 0)
                    return Results.NotFound();

                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Eliminar evento
            evento.MapDelete("/{eventoId}", (int eventoId, IEventoService eventoService) =>
            {
                var eliminado = eventoService.EliminarEvento(eventoId);
                if (eliminado == 0)
                    return Results.NotFound();

                return Results.NoContent();

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            // Publicar evento
            evento.MapPost("/{eventoId}/publicar", (int id, IEventoService eventoService) =>
            {
                var publicado = eventoService.Publicar(id);
                if (publicado == 0)
                    return Results.NotFound();

                return Results.Ok(new { mensaje = "Evento publicado correctamente" });

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });

            // Cancelar evento
            evento.MapPost("/{eventoId}/cancelar", (int id, IEventoService eventoService) =>
            {
                var cancelado = eventoService.Cancelar(id);
                if (cancelado == 0)
                    return Results.NotFound();

                return Results.Ok(new { mensaje = "Evento cancelado correctamente" });

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Organizador" });
        }
    }
}
