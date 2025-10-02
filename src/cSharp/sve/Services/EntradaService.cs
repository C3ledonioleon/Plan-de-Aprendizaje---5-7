using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services
{
    public class EntradaService : IEntradaService
    {
        private readonly IEntradaRepository _entradaRepository;

        public EntradaService(IEntradaRepository entradaRepository)
        {
            _entradaRepository = entradaRepository;
        }

        public List<EntradaDto> ObtenerTodo()
        {
            return _entradaRepository.GetAll()
                .Select(entrada => new EntradaDto
                {
                    IdEntrada = entrada.IdEntrada,
                    Precio = entrada.Precio,
                    IdOrden = entrada.IdOrden,
                    IdTarifa = entrada.IdTarifa,
                    Estado = entrada.Estado
                }).ToList();
        }

        public Entrada? ObtenerPorId(int id)
        {
            return _entradaRepository.GetById(id);
        }

        public int AgregarEntrada(EntradaCreateDto entrada)
        {
            var nuevaEntrada = new Entrada
            {
                Precio = entrada.Precio,
                IdOrden = entrada.IdOrden,
                IdTarifa = entrada.IdTarifa,
                Estado = EstadoEntrada.Activa,
                IdCliente = entrada.IdCliente,   // <-- Asignar valor
                IdFuncion = entrada.IdFuncion
            };
            return _entradaRepository.Add( nuevaEntrada);
        }
        public bool ActualizarEntrada(int id, EntradaUpdateDto entrada)
        {
             var existente = _entradaRepository.GetById(id);
            if (existente == null) return false;

            existente.Precio = entrada.Precio;
            existente.IdOrden = entrada.IdOrden;
            existente.IdTarifa = entrada.IdTarifa;
            existente.Estado = entrada.Estado;
            
            return _entradaRepository.Update(id,existente);
        }


                // Anular entrada
        public bool AnularEntrada(int id)
        {
            var entrada = _entradaRepository.GetById(id);
            if (entrada == null) return false;

            entrada.Estado = EstadoEntrada.Anulada;
            return _entradaRepository.Update(id, entrada);
        }
        public bool EliminarEntrada(int id)
        {
            return _entradaRepository.Delete(id);
        }
    }
}
