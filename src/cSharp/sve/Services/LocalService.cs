using sve.DTOs;
using sve.Models;
using sve.Repositories;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services
{
    public class LocalService : ILocalService
    {
        private readonly ILocalRepository _localRepository;

        public LocalService(ILocalRepository localRepository)
        {
            _localRepository = localRepository;
        }

        public List<LocalDto> ObtenerTodo()
            {
            return _localRepository.GetAll()
                .Select(local => new LocalDto
                {
                    IdLocal = local.IdLocal,
                    Nombre = local.Nombre,
                    Direccion = local.Direccion,
                    CapacidadTotal = local.CapacidadTotal
                }).ToList();
        }

        public Local? ObtenerPorId(int id) 
        {
            return _localRepository.GetById(id);
        }
        public int AgregarLocal(LocalCreateDto local)
        {
            var nuevoLocal = new Local
            {
                Nombre = local.Nombre,
                Direccion = local.Direccion,
                CapacidadTotal = local.CapacidadTotal
            };
            return _localRepository.Add(nuevoLocal);
        }

        public int ActualizarLocal(int id, LocalUpdateDto local)
        {
            var entidad = new Local
            {
                IdLocal = id,
                Nombre = local.Nombre,
                Direccion = local.Direccion,
                CapacidadTotal = local.CapacidadTotal
            };
            return _localRepository.Update(id, entidad);
        }

        public int EliminarLocal(int id) 
        {    
        return _localRepository.Delete(id);
        }
    }
}
