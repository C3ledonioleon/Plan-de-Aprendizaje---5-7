using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services
{
    public class TarifaService : ITarifaService
    {
        private readonly ITarifaRepository _tarifaRepository;

        public TarifaService(ITarifaRepository tarifaRepository)
        {
            _tarifaRepository = tarifaRepository;
        }

        // Devuelve todas las tarifas como DTO
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

        // Devuelve una tarifa por ID
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

        // Crea una tarifa nueva
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

        // Actualiza una tarifa existente
        public bool ActualizarTarifa(int id, TarifaUpdateDto tarifa)
        {
            var entidad = new Tarifa
            {
                Precio = tarifa.Precio,
                Stock = tarifa.Stock,
                Estado = tarifa.Estado
            };

            return _tarifaRepository.Update(id, entidad);
        }

        // Elimina una tarifa por ID
        public bool EliminarTarifa(int id)
        {
            return _tarifaRepository.Delete(id);
        }
    }
}
