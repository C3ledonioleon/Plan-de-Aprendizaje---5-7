using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services
{
    public class FuncionService : IFuncionService
    {  
        private readonly IFuncionRepository _funcionRepository;

        public FuncionService(IFuncionRepository funcionRepository)
        {
            _funcionRepository = funcionRepository;
        }

        public List<FuncionDto> ObtenerTodo()
        {
            return _funcionRepository.GetAll()
                .Select(funcion => new FuncionDto
                {
                    IdFuncion = funcion.IdFuncion,
                    IdEvento = funcion.IdEvento,
                    IdLocal = funcion.IdLocal,
                    FechaHora = funcion.FechaHora,
                    Estado = funcion.Estado
                }).ToList();
        }
        public Funcion? ObtenerPorId(int id)
        {
            return _funcionRepository.GetById(id);
        }

        public int AgregarFuncion(FuncionCreateDto funcion)
        {
            var nuevaFuncion = new Funcion
            {
                IdEvento = funcion.IdEvento,
                IdLocal = funcion.IdLocal,
                FechaHora = funcion.FechaHora,
                Estado = EstadoFuncion.Pendiente // Valor inicial
            };

            return _funcionRepository.Add(nuevaFuncion);
        }

        public bool ActualizarFuncion(int id, FuncionUpdateDto funcion)
        {
            var entidad = new Funcion
            {
                IdFuncion = id,
                IdEvento = funcion.IdEvento,
                IdLocal = funcion.IdLocal,
                FechaHora = funcion.FechaHora,
                Estado = funcion.Estado
            };

            return _funcionRepository.Update(id, entidad);
        }

        public bool EliminarFuncion(int id)
        {
            return _funcionRepository.Delete(id);
        }

        public bool CancelarFuncion(int id)
        {
            var funcion = _funcionRepository.GetById(id);
            if (funcion == null) return false;

            var funcionCancelada = new FuncionUpdateDto
            {
                IdEvento = funcion.IdEvento,
                IdLocal = funcion.IdLocal,
                FechaHora = funcion.FechaHora,
                Estado = EstadoFuncion.Cancelada
            };

            return ActualizarFuncion(id, funcionCancelada);
        }

        
    }
}
