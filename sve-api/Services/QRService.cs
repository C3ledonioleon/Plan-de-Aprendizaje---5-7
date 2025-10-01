using QRCoder;

namespace sve.Services;

public class QRService
{
    public string Saludo = "Hola desde el servicio de QR";

    public byte[] GenerarQR(string contenido)
    {
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            var pngQrCode = new PngByteQRCode(qrCodeData);
            return pngQrCode.GetGraphic(20); // devuelve byte[] directamente
        }
    }
}