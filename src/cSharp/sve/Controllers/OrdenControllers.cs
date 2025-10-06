using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Services.Contracts;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly IOrdenService _ordenService;

        public OrdenesController(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        // 🔹 Crear orden y reservar stock
        [HttpPost]
        public IActionResult CrearOrden([FromBody] OrdenCreateDto orden)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ordenId = _ordenService.AgregarOrden(orden);
            return Ok(new { IdOrden = ordenId });
        }

        // 🔹 Listar todas las órdenes
        [HttpGet]
        public IActionResult ObtenerTodas()
        {
            var ordenes = _ordenService.ObtenerTodo();
            return Ok(ordenes);
        }

        // 🔹 Detalle de una orden
        [HttpGet("{ordenId:int}")]
        public IActionResult ObtenerPorId(int ordenId)
        {
            var orden = _ordenService.ObtenerPorId(ordenId);
            if (orden == null)
                return NotFound();

            return Ok(orden);
        }

        // 🔹 Marcar orden como pagada y emitir entradas
        [HttpPost("{ordenId:int}/pagar")]
        public IActionResult PagarOrden(int ordenId)
        {
            var result = _ordenService.PagarOrden(ordenId);
            if (!result)
                return BadRequest("No se pudo procesar el pago o la orden no existe.");

            return Ok(new { Mensaje = "Orden pagada y entradas emitidas" });
        }

        // 🔹 Cancelar una orden (si está en estado Creada)
        [HttpPost("{ordenId:int}/cancelar")]
        public IActionResult CancelarOrden(int ordenId)
        {
            var result = _ordenService.CancelarOrden(ordenId);
            if (!result)
                return BadRequest("No se pudo cancelar la orden (puede que ya esté pagada o no exista).");

            return Ok(new { Mensaje = "Orden cancelada" });
        }
    }
}
