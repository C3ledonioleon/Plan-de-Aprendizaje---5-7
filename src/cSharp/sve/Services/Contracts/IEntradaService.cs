using sve.DTOs;
using sve.Models;

namespace sve.Services.Contracts;

public interface IEntradaService
{
    List<EntradaDto> ObtenerTodo();
    Entrada? ObtenerPorId(int id);
    int AgregarEntrada(EntradaCreateDto entrada);
    int ActualizarEntrada(int id, EntradaUpdateDto entrada);
    int EliminarEntrada(int id);
    bool AnularEntrada(int id);
    string ValidarQR(string contenido);
}
