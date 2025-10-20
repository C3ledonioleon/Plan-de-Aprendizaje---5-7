namespace sve.Services.Contracts;

public interface IQRService
{
    byte[] GenerarQR(string contenido);
}