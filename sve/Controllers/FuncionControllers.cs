using Microsoft.AspNetCore.Mvc;
using sve.Models;
using sve.DTOs;
using sve.Services.Contracts;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FuncionesController : ControllerBase
    {
        private readonly IFuncionService _funcionService;

        public FuncionesController(IFuncionService funcionService)
        {
            _funcionService = funcionService;
        }

        // POST /funciones — Crea una Función
        [HttpPost]
        public IActionResult CrearFuncion([FromBody] FuncionCreateDto dto)
        {
            var id = _funcionService.AgregarFuncion(dto);
            return CreatedAtAction(nameof(ObtenerFuncion), new { funcionId = id }, dto);
        }

        // GET /funciones — Lista funciones
        [HttpGet]
        public IActionResult ObtenerFunciones()
        {
            var funciones = _funcionService.ObtenerTodo();
            return Ok(funciones);
        }

        // GET /funciones/{funcionId} — Detalle de función
        [HttpGet("{funcionId}")]
        public IActionResult ObtenerFuncion(int funcionId)
        {
            var funcion = _funcionService.ObtenerPorId(funcionId);
            if (funcion == null) return NotFound();
            return Ok(funcion);
        }

        // PUT /funciones/{funcionId} — Actualiza función
        [HttpPut("{funcionId}")]
        public IActionResult ActualizarFuncion(int funcionId, [FromBody] FuncionUpdateDto dto)
        {
            var actualizado = _funcionService.ActualizarFuncion(funcionId, dto);
            if (!actualizado) return NotFound();
            return NoContent();
        }

        // DELETE /funciones/{funcionId} — Elimina función
        [HttpDelete("{funcionId}")]
        public IActionResult EliminarFuncion(int funcionId)
        {
            var eliminado = _funcionService.EliminarFuncion(funcionId);
            if (!eliminado) return NotFound();
            return NoContent();
        }

        // POST /funciones/{funcionId}/cancelar — Cancela la función
        [HttpPost("{funcionId}/cancelar")]
        public IActionResult CancelarFuncion(int funcionId)
        {
            var actualizado = _funcionService.CancelarFuncion(funcionId);
            if (!actualizado) return NotFound();
            return Ok(new { mensaje = "Función cancelada correctamente" });
        }
    }
}
