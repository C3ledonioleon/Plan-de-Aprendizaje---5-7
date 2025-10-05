using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Models;
using sve.Services.Contracts;
using System.Linq;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrdenesController : ControllerBase
    {
        private readonly IOrdenService _ordenService;

        public OrdenesController(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        // POST /ordenes — Crea una orden
        [HttpPost]
        public IActionResult CrearOrden([FromBody] OrdenCreateDto orden)
        {
            var id = _ordenService.AgregarOrden(orden);
            return CreatedAtAction(nameof(ObtenerOrden), new { ordenId = id }, orden);
        }

        // GET /ordenes — Lista todas las órdenes
        [HttpGet]
        public IActionResult ObtenerOrdenes()
        {
            var ordenes = _ordenService.ObtenerTodo();
            return Ok(ordenes);
        }

        // GET /ordenes/{ordenId} — Detalle de una orden
        [HttpGet("{ordenId}")]
        public IActionResult ObtenerOrden(int ordenId)
        {
            var orden = _ordenService.ObtenerPorId(ordenId);
            if (orden == null) return NotFound();
            return Ok(orden);
        }

        // PUT /ordenes/{ordenId} — Actualiza una orden
        [HttpPut("{ordenId}")]
        public IActionResult ActualizarOrden(int ordenId, [FromBody] OrdenUpdateDto orden)
        {
            var actualizado = _ordenService.ActualizarOrden(ordenId, orden);
            if (actualizado == 0) return NotFound();
            return NoContent();
        }
    }
}

        // DELETE /ordenes/{ordenId} — Elimina una orden
