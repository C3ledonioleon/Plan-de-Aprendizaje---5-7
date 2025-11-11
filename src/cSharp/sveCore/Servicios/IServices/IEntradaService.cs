using sveCore.DTOs;
using sveCore.Models;

namespace sveCore.Services.IServices;
public interface IEntradaService
{
    List<EntradaDto> ObtenerTodo();
    Entrada? ObtenerPorId(int id);

    int ActualizarEntrada(int id, EntradaUpdateDto entrada);
    int EliminarEntrada(int id);
    bool AnularEntrada(int id);
    string ValidarQR(string contenido);
}
