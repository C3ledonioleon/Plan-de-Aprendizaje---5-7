using sveCore.DTOs;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveCore.Services.IServices;

namespace sveServices.Services;

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
                Local = sector.Local,
                Tarifas = sector.Tarifas 
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
        return _sectorRepository.Update(entidad);
    }

    public int EliminarSector(int id)
    {
        return _sectorRepository.Delete(id);
    }
}
