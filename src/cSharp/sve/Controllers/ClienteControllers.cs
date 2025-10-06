using Microsoft.AspNetCore.Mvc;
using sve.Services.Contracts;
using sve.DTOs;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public IActionResult CrearCliente([FromBody] ClienteCreateDto cliente)
        {
            var id = _clienteService.AgregarCliente(cliente);
            return CreatedAtAction(nameof(ObtenerClientePorId), new { clienteId = id }, cliente);
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
        public IActionResult ActualizarCliente(int clienteId, [FromBody] ClienteUpdateDto cliente)
        {
            var actualizado = _clienteService.ActualizarCliente(clienteId, cliente);
            if (actualizado == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
