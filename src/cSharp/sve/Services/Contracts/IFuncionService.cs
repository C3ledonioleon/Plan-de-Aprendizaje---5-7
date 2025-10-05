using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts;

public interface IFuncionService
{
    List<FuncionDto> ObtenerTodo();
    Funcion? ObtenerPorId(int id);
    int AgregarFuncion(FuncionCreateDto funcion);
    int ActualizarFuncion(int id, FuncionUpdateDto funcion); // <-- DTO en vez de modelo
    int EliminarFuncion(int id);
    int CancelarFuncion(int id); // opcional para simplificar el controller
}
