using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Models;
using sve.Services.Contracts;
using System.Text;
using System.Security.Cryptography;
using QRCoder;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradaService _entradaService;
        private static readonly string LlaveQR = "Efrain123!";

        public EntradasController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }

        [HttpGet]
        public ActionResult<List<EntradaDto>> ObtenerEntradas() => Ok(_entradaService.ObtenerTodo());

        [HttpGet("{entradaId}")]
        public ActionResult<EntradaDto> ObtenerEntradaPorId(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            return entrada == null ? NotFound($"No se encontró la entrada con ID {entradaId}") : Ok(entrada);
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
            return actualizado == 0 ? NotFound($"No se encontró la entrada con ID {entradaId}") : NoContent();
        }

        [HttpPost("{entradaId}/anular")]
        public IActionResult AnularEntrada(int entradaId)
        {
            return !_entradaService.AnularEntrada(entradaId) ? NotFound($"No se encontró la entrada con ID {entradaId}") :
                Ok($"La entrada con ID {entradaId} fue anulada correctamente.");
        }

        [HttpDelete("{entradaId}")]
        public IActionResult EliminarEntrada(int entradaId)
        {
            var eliminado = _entradaService.EliminarEntrada(entradaId);
            return eliminado == 0 ? NotFound($"No se encontró la entrada con ID {entradaId}") : NoContent();
        }

        [HttpGet("{entradaId}/qr")]
        public IActionResult GenerarQR(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            if (entrada == null) return NotFound("noExiste");

            var qrBytes = GenerarQRBytes(EncriptarId(entrada.IdEntrada));
            return File(qrBytes, "image/png");
        }

        [HttpGet("{entradaId}/qr-texto")]
        public IActionResult GenerarQRTexto(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            if (entrada == null) return NotFound("noExiste");
            return Ok(new { qr = EncriptarId(entrada.IdEntrada) });
        }
        public class ValidarQrRequest { public string Qr { get; set; } }
        [HttpPost("validar")]
        public IActionResult ValidarQR([FromBody] ValidarQrRequest request)
        {
            var id = DesencriptarId(request.Qr);
            if (id == null) return BadRequest("firmaNoValida");

            var entrada = _entradaService.ObtenerPorId(id.Value);
            if (entrada == null) return NotFound("noExiste");

            return entrada.Estado switch
            {
                EstadoEntrada.Activa => Ok("Activa"),
                EstadoEntrada.Usado => Ok("Usado"),
                EstadoEntrada.Vencido => Ok("Expirado"),
                _ => BadRequest("EstadoDesconocido")
            };
        }
        [HttpPost("{entradaId}/usar")]
        public IActionResult UsarQR(int entradaId)
        {
            var entrada = _entradaService.ObtenerPorId(entradaId);
            if (entrada == null) return NotFound("noExiste");
            if (entrada.Estado == EstadoEntrada.Usado) return BadRequest("Usado");
            if (entrada.Estado == EstadoEntrada.Vencido) return BadRequest("Expirado");

            entrada.Estado = EstadoEntrada.Usado;
            _entradaService.ActualizarEntrada(entradaId, new EntradaUpdateDto
            {
                Precio = entrada.Precio,
                IdOrden = entrada.IdOrden,
                IdTarifa = entrada.IdTarifa,
                IdCliente = entrada.IdCliente,
                IdFuncion = entrada.IdFuncion,
                Estado = entrada.Estado
            });

            return Ok("Ok");
        }

        private string EncriptarId(int id)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(LlaveQR.PadRight(32).Substring(0, 32));
            aes.IV = new byte[16];
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var buffer = Encoding.UTF8.GetBytes(id.ToString());
            var encrypted = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            return Convert.ToBase64String(encrypted);
        }

        private int? DesencriptarId(string encrypted)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(LlaveQR.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var buffer = Convert.FromBase64String(encrypted);
                var decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                return int.Parse(Encoding.UTF8.GetString(decrypted));
            }
            catch { return null; }
        }
        private byte[] GenerarQRBytes(string contenido)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            return qrCode.GetGraphic(20);
        }
    }
}
