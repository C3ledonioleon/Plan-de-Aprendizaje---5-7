using sveCore.DTOs;
using sveCore.Models;
using System.Collections.Generic;

namespace sveCore.Services.IServices;

    public interface ISectorService
    {
        List<SectorDto> ObtenerTodo();
        Sector? ObtenerPorId(int id);
        int AgregarSector(SectorCreateDto sector);         // Usar DTO para creación
        int ActualizarSector(int id, SectorUpdateDto sector);  // Usar DTO para actualización
        int EliminarSector(int id);
    }

