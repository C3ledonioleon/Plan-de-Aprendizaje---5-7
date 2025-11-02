namespace sveCore.Services.IServices;

public interface IQRService
{
    byte[] GenerarQR(string contenido);
}