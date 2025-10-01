using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

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
                .Select(t => new TarifaDto
                {
                    IdTarifa = t.IdTarifa,
                    IdFuncion = t.IdFuncion,
                    IdSector = t.IdSector,
                    Precio = t.Precio,
                    Stock = t.Stock,
                    Estado = t.Estado,
                    Funcion = t.Funcion,
                    Sector = t.Sector
                }).ToList();
        }

        // Devuelve una tarifa por ID
        public TarifaDto? ObtenerPorId(int id)
        {
            var t = _tarifaRepository.GetById(id);
            if (t == null) return null;

            return new TarifaDto
            {
                IdTarifa = t.IdTarifa,
                IdFuncion = t.IdFuncion,
                IdSector = t.IdSector,
                Precio = t.Precio,
                Stock = t.Stock,
                Estado = t.Estado,
                Funcion = t.Funcion,
                Sector = t.Sector
            };
        }

        // Crea una tarifa nueva
        public int AgregarTarifa(TarifaCreateDto dto)
        {
            var tarifa = new Tarifa
            {
                IdFuncion = dto.IdFuncion,
                IdSector = dto.IdSector,
                Precio = dto.Precio,
                Stock = dto.Stock,
                Estado = dto.Estado
            };

            return _tarifaRepository.Add(tarifa);
        }

        // Actualiza una tarifa existente
        public bool ActualizarTarifa(int id, TarifaUpdateDto dto)
        {
            var tarifa = new Tarifa
            {
                Precio = dto.Precio,
                Stock = dto.Stock,
                Estado = dto.Estado
            };

            return _tarifaRepository.Update(id, tarifa);
        }

        // Elimina una tarifa por ID
        public bool EliminarTarifa(int id)
        {
            return _tarifaRepository.Delete(id);
        }
    }
}
