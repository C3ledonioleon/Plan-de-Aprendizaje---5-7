using sveCore.DTOs;
using sveCore.Models;

namespace sveCore.Services.IServices;
public interface IFuncionService
{
    List<FuncionDto> ObtenerTodo();
    Funcion? ObtenerPorId(int id);
    int AgregarFuncion(FuncionCreateDto funcion);
    int ActualizarFuncion(int id, FuncionUpdateDto funcion); // <-- DTO en vez de modelo
    int EliminarFuncion(int id);
    int CancelarFuncion(int id); // opcional para simplificar el controller
}
