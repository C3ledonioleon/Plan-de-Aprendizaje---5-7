using sveCore.DTOs;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveCore.Services.IServices;

namespace sveServices.Services;

public class TarifaService : ITarifaService
{
    private readonly ITarifaRepository _tarifaRepository;

    public TarifaService(ITarifaRepository tarifaRepository)
    {
        _tarifaRepository = tarifaRepository;
    }

    public List<TarifaDto> ObtenerTodo()
    {
        return _tarifaRepository.GetAll()
            .Select(tarifa => new TarifaDto
            {
                IdTarifa = tarifa.IdTarifa,
                IdFuncion = tarifa.IdFuncion,
                IdSector = tarifa.IdSector,
                Precio = tarifa.Precio,
                Stock = tarifa.Stock,
                Estado = tarifa.Estado,
                Funcion = tarifa.Funcion,
                Sector = tarifa.Sector
            }).ToList();
    }

    public TarifaDto? ObtenerPorId(int id)
    {
        var tarifa = _tarifaRepository.GetById(id);
        if (tarifa == null) return null;

        return new TarifaDto
        {
            IdTarifa = tarifa.IdTarifa,
            IdFuncion = tarifa.IdFuncion,
            IdSector = tarifa.IdSector,
            Precio = tarifa.Precio,
            Stock = tarifa.Stock,
            Estado = tarifa.Estado,
            Funcion = tarifa.Funcion,
            Sector = tarifa.Sector
        };
    }

    public int AgregarTarifa(TarifaCreateDto tarifa)
    {
        var nuevaTarifa = new Tarifa
        {
            IdFuncion = tarifa.IdFuncion,
            IdSector = tarifa.IdSector,
            Precio = tarifa.Precio,
            Stock = tarifa.Stock,
            Estado = tarifa.Estado
        };

        return _tarifaRepository.Add(nuevaTarifa);
    }

    public int ActualizarTarifa(int id, TarifaUpdateDto tarifa)
    {
        var entidad = new Tarifa
        {
            Precio = tarifa.Precio,
            Stock = tarifa.Stock,
            Estado = tarifa.Estado
        };

        return _tarifaRepository.Update(entidad);
    }

    public int EliminarTarifa(int id)
    {
        return _tarifaRepository.Delete(id);
    }
}
