using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;
using System.Collections.Generic;

namespace sve.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _ordenRepository;

        public OrdenService(IOrdenRepository ordenRepository)
        {
            _ordenRepository = ordenRepository;
        }

        public List<Orden> ObtenerTodo()
        {
            return _ordenRepository.GetAll();
        }

        public Orden? ObtenerPorId(int id)
        {
            return _ordenRepository.GetById(id);
        }

        public int AgregarOrden(OrdenCreateDto dto)
        {
            var nuevaOrden = new Orden
            {
                IdCliente = dto.IdCliente,
                IdTarifa = dto.IdTarifa,
                Total = dto.Total,
                Fecha = dto.Fecha,
                Estado = EstadoOrden.Creada
            };

            return _ordenRepository.Add(nuevaOrden);
        }

        public bool ActualizarOrden(int id, OrdenUpdateDto dto)
        {
            var orden = new Orden
            {
                IdOrden = id,
                IdCliente = dto.IdCliente,
                IdTarifa = dto.IdTarifa,
                Total = dto.Total,
                Fecha = dto.Fecha,
                Estado = dto.Estado
            };

            return _ordenRepository.Update(id, orden);
        }

        public bool EliminarOrden(int id)
        {
            return _ordenRepository.Delete(id);
        }
    }
}
