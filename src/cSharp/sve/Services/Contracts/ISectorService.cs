using sve.Models;
using sve.DTOs;
using System.Collections.Generic;

namespace sve.Services.Contracts;

    public interface ISectorService
    {
        List<SectorDto> ObtenerTodo();
        Sector? ObtenerPorId(int id);
        int AgregarSector(SectorCreateDto sector);         // Usar DTO para creación
        bool ActualizarSector(int id, SectorUpdateDto sector);  // Usar DTO para actualización
        bool EliminarSector(int id);
    }

