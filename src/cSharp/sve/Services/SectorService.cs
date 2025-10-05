using sve.Models;
using sve.DTOs;
using sve.Repositories.Contracts;
using sve.Services.Contracts;
using System.Collections.Generic;

namespace sve.Services
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;

        public SectorService(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        public List<SectorDto> ObtenerTodo()
        {
            return _sectorRepository.GetAll()
                .Select(sector => new SectorDto
                {
                    IdSector = sector.IdSector,
                    Nombre = sector.Nombre,
                    Capacidad = sector.Capacidad,
                    IdLocal = sector.IdLocal,
                    Local = sector.Local,      // relación con Local
                    Tarifas = sector.Tarifas   // lista de tarifas asociadas
                }).ToList();
        }

        public Sector? ObtenerPorId(int id)
        { 
            return _sectorRepository.GetById(id); 
        }

        public int AgregarSector(SectorCreateDto sector)
        {
            var nuevoSector = new Sector
            {
                Nombre = sector.Nombre,
                Capacidad = sector.Capacidad,
                IdLocal = sector.IdLocal
            };
            return _sectorRepository.Add(nuevoSector);
        }

        public int ActualizarSector(int id, SectorUpdateDto sector)
        {
            var entidad = new Sector
            {
                IdSector = id,
                Nombre = sector.Nombre,
                Capacidad = sector.Capacidad,
                IdLocal = sector.IdLocal
            };
            return _sectorRepository.Update(id, entidad);
        }

        public int EliminarSector(int id)
        {
            return _sectorRepository.Delete(id);
        }
    }
}
