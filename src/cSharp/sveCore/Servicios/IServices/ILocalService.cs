using sveCore.DTOs;
using sveCore.Models;
using System.Collections.Generic;

namespace sveCore.Services.IServices;
    public interface ILocalService
    {
        List<LocalDto> ObtenerTodo();
        Local? ObtenerPorId(int id);
        int AgregarLocal(LocalCreateDto local);         // Usar DTO para creación
        int ActualizarLocal(int id, LocalUpdateDto local);  // Usar DTO para actualización
        int EliminarLocal(int id);
    }

