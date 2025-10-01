using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Models;
using sve.Services.Contracts;
using System.Linq;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SectoresController : ControllerBase
    {
        private readonly ISectorService _sectorService;

        public SectoresController(ISectorService sectorService)
        {
            _sectorService = sectorService;
        }

        // POST /sectores — Crea un sector
        [HttpPost]
        public IActionResult CrearSector([FromBody] SectorCreateDto dto)
        {
            var id = _sectorService.AgregarSector(dto);
            return CreatedAtAction(nameof(ObtenerSector), new { sectorId = id }, dto);
        }

        // GET /sectores — Lista todos los sectores
        [HttpGet]
        public IActionResult ObtenerSectores()
        {
            var sectores = _sectorService.ObtenerTodo();
            return Ok(sectores);
        }

        // GET /sectores/{sectorId} — Detalle de un sector
        [HttpGet("{sectorId}")]
        public IActionResult ObtenerSector(int sectorId)
        {
            var sector = _sectorService.ObtenerPorId(sectorId);
            if (sector == null) return NotFound();
            return Ok(sector);
        }

        // PUT /sectores/{sectorId} — Actualiza un sector
        [HttpPut("{sectorId}")]
        public IActionResult ActualizarSector(int sectorId, [FromBody] SectorUpdateDto dto)
        {
            var actualizado = _sectorService.ActualizarSector(sectorId, dto);
            if (!actualizado) return NotFound();
            return NoContent();
        }

        // DELETE /sectores/{sectorId} — Elimina un sector
        [HttpDelete("{sectorId}")]
        public IActionResult EliminarSector(int sectorId)
        {
            var eliminado = _sectorService.EliminarSector(sectorId);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
