using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Services.Contracts;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradaService _entradaService;

        public EntradasController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }

        // GET /api/entradas — Listar todas las entradas
        [HttpGet]
        public ActionResult<List<EntradaDto>> ObtenerEntradas()
        {
            var entradas = _entradaService.ObtenerTodo();
            return Ok(entradas);
        }

        // GET /api/entradas/{entradaId} — Obtener entrada por ID
        [HttpGet("{entradaId}")]
        public ActionResult<EntradaDto> ObtenerEntradaPorId(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            if (entrada == null)
                return NotFound($"No se encontró la entrada con ID {entradaId}");

            return Ok(entrada);
        }

        // POST /api/entradas — Crear nueva entrada
        [HttpPost]
        public ActionResult CrearEntrada([FromBody] EntradaCreateDto entrada)
        {
            var id = _entradaService.AgregarEntrada(entrada);
            return CreatedAtAction(nameof(ObtenerEntradaPorId), new { entradaId = id }, entrada);
        }

        // PUT /api/entradas/{entradaId} — Actualizar entrada
        [HttpPut("{entradaId}")]
        public IActionResult ActualizarEntrada(int entradaId, [FromBody] EntradaUpdateDto entrada)
        {
            var actualizado = _entradaService.ActualizarEntrada(entradaId, entrada);
            if (!actualizado) return NotFound($"No se encontró la entrada con ID {entradaId}");

            return NoContent();
        }

        // POST /api/entradas/{entradaId}/anular — Anular entrada
        [HttpPost("{entradaId}/anular")]
        public IActionResult AnularEntrada(int entradaId)
        {
            var resultado = _entradaService.AnularEntrada(entradaId);
            if (!resultado) return NotFound($"No se encontró la entrada con ID {entradaId}");

            return Ok($"La entrada con ID {entradaId} fue anulada correctamente.");
        }

        // DELETE /api/entradas/{entradaId} — Eliminar entrada
        [HttpDelete("{entradaId}")]
        public IActionResult EliminarEntrada(int entradaId)
        {
            var eliminado = _entradaService.EliminarEntrada(entradaId);
            if (!eliminado) return NotFound($"No se encontró la entrada con ID {entradaId}");

            return NoContent();
        }
    }
}
