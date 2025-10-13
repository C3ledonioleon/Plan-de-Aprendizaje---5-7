using Microsoft.AspNetCore.Mvc;
using sve.Services; // <- tu namespace donde está QRService

[ApiController]
[Route("api/entradas")]
public class EntradasController : ControllerBase
{
    private readonly QRService _qrService;

    // Inyección de QRService por constructor
    public EntradasController(QRService qrService)
    {
        _qrService = qrService;
    }

    // Endpoint para obtener el QR de una entrada
    [HttpGet("{entradaId}/qr")]
    public IActionResult ObtenerQR(int entradaId)
    {
        //se debe verificar que la orden de la entrada se encuentre pagada
        string contenidoQR = $"https://tusistema.com/entrada/{entradaId}";
        byte[] qrBytes = _qrService.GenerarQR(contenidoQR);

        return File(qrBytes, "image/png");
    }
}