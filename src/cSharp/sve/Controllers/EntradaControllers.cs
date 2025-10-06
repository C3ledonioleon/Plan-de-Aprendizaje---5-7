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

        [HttpGet]
        public ActionResult<List<EntradaDto>> ObtenerEntradas()
        {
            var entradas = _entradaService.ObtenerTodo();
            return Ok(entradas);
        }

        [HttpGet("{entradaId}")]
        public ActionResult<EntradaDto> ObtenerEntradaPorId(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            if (entrada == null)
                return NotFound($"No se encontró la entrada con ID {entradaId}");

            return Ok(entrada);
        }

        [HttpPost]
        public ActionResult CrearEntrada([FromBody] EntradaCreateDto entrada)
        {
            var id = _entradaService.AgregarEntrada(entrada);
            return CreatedAtAction(nameof(ObtenerEntradaPorId), new { entradaId = id }, entrada);
        }

        [HttpPut("{entradaId}")]
        public IActionResult ActualizarEntrada(int entradaId, [FromBody] EntradaUpdateDto entrada)
        {
            var actualizado = _entradaService.ActualizarEntrada(entradaId, entrada);
            if (actualizado == 0) return NotFound($"No se encontró la entrada con ID {entradaId}");

            return NoContent();
        }

        [HttpPost("{entradaId}/anular")]
        public IActionResult AnularEntrada(int entradaId)
        {
            var resultado = _entradaService.AnularEntrada(entradaId);
            if (resultado == false) return NotFound($"No se encontró la entrada con ID {entradaId}");

            return Ok($"La entrada con ID {entradaId} fue anulada correctamente.");
        }

        [HttpDelete("{entradaId}")]
        public IActionResult EliminarEntrada(int entradaId)
        {
            var eliminado = _entradaService.EliminarEntrada(entradaId);
            if (eliminado == 0) return NotFound($"No se encontró la entrada con ID {entradaId}");

            return NoContent();
        }
    }
}
