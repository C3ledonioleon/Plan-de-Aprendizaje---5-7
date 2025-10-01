using Microsoft.AspNetCore.Mvc;
using sve.Models;
using sve.Services.Contracts;
using sve.DTOs;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public IActionResult CrearCliente([FromBody] ClienteCreateDto dto)
        {

            var id = _clienteService.AgregarCliente(dto);
            return CreatedAtAction(nameof(ObtenerClientePorId), new { clienteId = id }, dto);
        }

        [HttpGet]
        public IActionResult ObtenerClientes()
        {
            var clientes = _clienteService.ObtenerTodo();
            return Ok(clientes);
        }

        // GET /clientes/{clienteId} — Detalle de cliente
        [HttpGet("{clienteId}")]
        public IActionResult ObtenerClientePorId(int clienteId)
        {
            var cliente = _clienteService.ObtenerPorId(clienteId);
            if (cliente == null)
                return NotFound();
            return Ok(cliente);
        }

        // PUT /clientes/{clienteId} — Actualiza datos
         [HttpPut("{clienteId}")]
          public IActionResult ActualizarCliente(int clienteId, [FromBody] ClienteUpdateDto dto)
       {
       // Llamamos al servicio para actualizar directamente
        var actualizado = _clienteService.ActualizarCliente(clienteId, dto);
      if (!actualizado)
        {   
        // Si el cliente no existe, respondemos con 404
        return NotFound();
        }
        // Si se actualizó correctamente, respondemos 204 No Content
        return NoContent();
        }

    }
}
