using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts
{
    public interface IUsuarioRepository
    {
        List<UsuarioDto> ObtenerTodo();
        TarifaDto? ObtenerPorId(int id);
        int AgregarTarifa(TarifaCreateDto tarifa);
        bool ActualizarTarifa(int id, TarifaUpdateDto tarifa);
        bool EliminarTarifa(int id);
    }
}
