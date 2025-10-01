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

        public List<Funcion> ObtenerTodo()
        {
            return _funcionRepository.GetAll();
        }

        public Funcion? ObtenerPorId(int id)
        {
            return _funcionRepository.GetById(id);
        }

        public int AgregarFuncion(FuncionCreateDto dto)
        {
            var nuevaFuncion = new Funcion
            {
                IdEvento = dto.IdEvento,
                IdLocal = dto.IdLocal,
                FechaHora = dto.FechaHora,
                Estado = EstadoFuncion.Pendiente // Valor inicial
            };

            return _funcionRepository.Add(nuevaFuncion);
        }

        public bool ActualizarFuncion(int id, FuncionUpdateDto dto)
        {
            var entidad = new Funcion
            {
                IdFuncion = id,
                IdEvento = dto.IdEvento,
                IdLocal = dto.IdLocal,
                FechaHora = dto.FechaHora,
                Estado = dto.Estado
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

            var dto = new FuncionUpdateDto
            {
                IdEvento = funcion.IdEvento,
                IdLocal = funcion.IdLocal,
                FechaHora = funcion.FechaHora,
                Estado = EstadoFuncion.Cancelada
            };

            return ActualizarFuncion(id, dto);
        }
    }
}
