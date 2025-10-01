using Microsoft.AspNetCore.Mvc;
using sve.Models;
using sve.Services.Contracts;
using sve.DTOs;

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

        // GET /api/entradas
        [HttpGet]
        public ActionResult<List<Entrada>> ObtenerEntradas()
        {
            var entradas = _entradaService.ObtenerTodo();
            return Ok(entradas);
        }

        // GET /api/entradas/{entradaId}
        [HttpGet("{entradaId}")]
        public ActionResult<Entrada> ObtenerEntradaPorId(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            if (entrada == null)
                return NotFound($"No se encontró la entrada con ID {entradaId}");

            return Ok(entrada);
        }

        // POST /api/entradas/{entradaId}/anular
// POST /api/entradas/{entradaId}/anular
[HttpPost("{entradaId}/anular")]
public ActionResult AnularEntrada(int entradaId)
{
    var resultado = _entradaService.AnularEntrada(entradaId);
    if (!resultado)
        return NotFound($"No se encontró la entrada con ID {entradaId}");

    return Ok($"La entrada con ID {entradaId} fue anulada correctamente.");
}
    }
}
