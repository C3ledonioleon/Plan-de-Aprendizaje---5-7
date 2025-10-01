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

        public List<Sector> ObtenerTodo() => _sectorRepository.GetAll();

        public Sector? ObtenerPorId(int id) => _sectorRepository.GetById(id);

        public int AgregarSector(SectorCreateDto dto)
        {
            var nuevoSector = new Sector
            {
                Nombre = dto.Nombre,
                Capacidad = dto.Capacidad,
                IdLocal = dto.IdLocal
            };
            return _sectorRepository.Add(nuevoSector);
        }

        public bool ActualizarSector(int id, SectorUpdateDto dto)
        {
            var sector = new Sector
            {
                IdSector = id,
                Nombre = dto.Nombre,
                Capacidad = dto.Capacidad,
                IdLocal = dto.IdLocal
            };
            return _sectorRepository.Update(id, sector);
        }

        public bool EliminarSector(int id)
        {
            return _sectorRepository.Delete(id);
        }
    }
}
