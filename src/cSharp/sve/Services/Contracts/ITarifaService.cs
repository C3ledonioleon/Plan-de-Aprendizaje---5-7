using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts
{
    public interface ITarifaService
    {
        List<TarifaDto> ObtenerTodo();
        TarifaDto? ObtenerPorId(int id);
        int AgregarTarifa(TarifaCreateDto tarifa);
        int ActualizarTarifa(int id, TarifaUpdateDto tarifa);
        int EliminarTarifa(int id);
    }
}
