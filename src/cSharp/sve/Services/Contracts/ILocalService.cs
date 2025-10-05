using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts;

    public interface ILocalService
    {
        List<LocalDto> ObtenerTodo();
        Local? ObtenerPorId(int id);
        int AgregarLocal(LocalCreateDto local);         // Usar DTO para creación
        int ActualizarLocal(int id, LocalUpdateDto local);  // Usar DTO para actualización
        int EliminarLocal(int id);
    }

