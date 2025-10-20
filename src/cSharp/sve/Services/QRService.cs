using QRCoder;
using sve.Services.Contracts;

namespace sve.Services;

public class QRService : IQRService
{
    public byte[] GenerarQR(string contenido)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrCodeData);
        return pngQrCode.GetGraphic(20); // Devuelve el QR como PNG en bytes
    }
}
