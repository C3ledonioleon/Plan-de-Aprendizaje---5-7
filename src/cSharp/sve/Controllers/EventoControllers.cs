using Microsoft.AspNetCore.Mvc;
using sveCore.DTOs;
using sveCore.Models;
using sveCore.Services.IServices;


namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpPost]
        public IActionResult CrearEvento([FromBody] EventoCreateDto evento)
        {
            var id = _eventoService.AgregarEvento(evento);
            return CreatedAtAction(nameof(ObtenerEvento), new { eventoId = id }, evento);
        }

        [HttpGet]
        public IActionResult ObtenerEventos()
        {
            var eventos = _eventoService.ObtenerTodo();
            return Ok(eventos);
        }

        [HttpGet("{eventoId}")]
        public IActionResult ObtenerEvento(int eventoId)
        {
            var evento = _eventoService.ObtenerPorId(eventoId);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpPut("{eventoId}")]
        public IActionResult ActualizarEvento(int eventoId, [FromBody] EventoUpdateDto evento)
        {
            var actualizado = _eventoService.ActualizarEvento(eventoId, evento);
            if (actualizado == 0) return NotFound();
            return NoContent();
        }

        [HttpDelete("{eventoId}")]
        public IActionResult EliminarEvento(int eventoId)
        {
            var eliminado = _eventoService.EliminarEvento(eventoId);
            if (eliminado == 0) return NotFound();
            return NoContent();
        }

        [HttpPost("{eventoId}/publicar")]
        public IActionResult PublicarEvento(int eventoId)
        {
            var publicado = _eventoService.Publicar(eventoId);
            if (publicado == 0) return NotFound();
            return Ok(new { mensaje = "Evento publicado correctamente" });
        }

        [HttpPost("{eventoId}/cancelar")]
        public IActionResult CancelarEvento(int eventoId)
        {
            var cancelado = _eventoService.Cancelar(eventoId);
            if (cancelado == 0) return NotFound();
            return Ok(new { mensaje = "Evento cancelado correctamente" });
        }
    }
}
