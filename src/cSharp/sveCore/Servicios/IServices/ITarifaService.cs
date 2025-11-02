using sveCore.DTOs;
using sveCore.Models;
using System.Collections.Generic;

namespace sveCore.Services.IServices;

    public interface ITarifaService
    {
        List<TarifaDto> ObtenerTodo();
        TarifaDto? ObtenerPorId(int id);
        int AgregarTarifa(TarifaCreateDto tarifa);
        int ActualizarTarifa(int id, TarifaUpdateDto tarifa);
        int EliminarTarifa(int id);
    }

