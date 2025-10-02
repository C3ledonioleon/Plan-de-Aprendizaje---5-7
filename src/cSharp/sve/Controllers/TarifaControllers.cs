using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Services.Contracts;
using System.Linq;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TarifasController : ControllerBase
    {
        private readonly ITarifaService _tarifaService;

        public TarifasController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        // POST /tarifas — Crea una Tarifa
        [HttpPost]
        public IActionResult CrearTarifa([FromBody] TarifaCreateDto tarifa)
        {
            var id = _tarifaService.AgregarTarifa(tarifa);
            return CreatedAtAction(nameof(ObtenerTarifa), new { tarifaId = id }, tarifa);
        }

        // GET /tarifas — Lista todas las tarifas
        [HttpGet]
        public IActionResult ObtenerTarifas()
        {
            var tarifas = _tarifaService.ObtenerTodo();
            return Ok(tarifas);
        }

        // GET /tarifas/{tarifaId} — Detalle de tarifa
        [HttpGet("{tarifaId}")]
        public IActionResult ObtenerTarifa(int tarifaId)
        {
            var tarifa = _tarifaService.ObtenerPorId(tarifaId);
            if (tarifa == null) return NotFound();
            return Ok(tarifa);
        }

        // PUT /tarifas/{tarifaId} — Actualiza precio, stock o estado
        [HttpPut("{tarifaId}")]
        public IActionResult ActualizarTarifa(int tarifaId, [FromBody] TarifaUpdateDto tarifa)
        {
            var actualizado = _tarifaService.ActualizarTarifa(tarifaId, tarifa);
            if (!actualizado) return NotFound();
            return NoContent();
        }

        // DELETE /tarifas/{tarifaId} — Elimina una tarifa
        [HttpDelete("{tarifaId}")]
        public IActionResult EliminarTarifa(int tarifaId)
        {
            var eliminado = _tarifaService.EliminarTarifa(tarifaId);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
