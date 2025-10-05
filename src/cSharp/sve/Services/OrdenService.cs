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

        public List<OrdenDto> ObtenerTodo()

        {
            return _ordenRepository.GetAll()
                .Select(orden => new OrdenDto
                {
                    IdOrden = orden.IdOrden,
                    IdTarifa = orden.IdTarifa,
                    IdCliente = orden.IdCliente,
                    Total = orden.Total,
                    Fecha = orden.Fecha,
                    Estado = orden.Estado,
                    Cliente = orden.Cliente,   // suponiendo que la entidad incluye la navegación
                    Tarifa = orden.Tarifa,     // idem
                    Entradas = orden.Entradas  // idem
                }).ToList();
        }

        public Orden? ObtenerPorId(int id)
        {
            return _ordenRepository.GetById(id);
        }

        public int AgregarOrden(OrdenCreateDto orden)
        {
            var nuevaOrden = new Orden
            {
                IdCliente = orden.IdCliente,
                IdTarifa = orden.IdTarifa,
                Total = orden.Total,
                Fecha = orden.Fecha,
                Estado = EstadoOrden.Creada
            };

            return _ordenRepository.Add(nuevaOrden);
        }

        public int ActualizarOrden(int id, OrdenUpdateDto orden)
        {
            var entidad = new Orden
            {
                IdOrden = id,
                IdCliente = orden.IdCliente,
                IdTarifa = orden.IdTarifa,
                Total = orden.Total,
                Fecha = orden.Fecha,
                Estado = orden.Estado
            };

            return _ordenRepository.Update(id, entidad);
        }

        public int EliminarOrden(int id)
        {
            return _ordenRepository.Delete(id);
        }
    }
}
