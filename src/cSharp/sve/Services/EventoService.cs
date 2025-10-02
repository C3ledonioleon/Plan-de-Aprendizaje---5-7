using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public List<EventoDto> ObtenerTodo()
        {
            var eventos = _eventoRepository.GetAll();
            return eventos.Select(evento => new EventoDto
            {
                IdEvento = evento.IdEvento,
                Nombre = evento.Nombre,
                Descripcion = evento.Descripcion,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                Estado = evento.Estado
            }).ToList();
        }

        public Evento? ObtenerPorId(int id)
        {
            var evento = _eventoRepository.GetById(id);
            if (evento == null) return null;
            return new Evento
            {
                IdEvento = evento.IdEvento,
                Nombre = evento.Nombre,
                Descripcion = evento.Descripcion,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                Estado = evento.Estado
            };
        }

        public int AgregarEvento(EventoCreateDto evento)
        {
            var nuevoEvento = new Evento
            {
                Nombre = evento.Nombre,
                Descripcion = evento.Descripcion,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                Estado = EstadoEvento.Inactivo
            };
            return _eventoRepository.Add(nuevoEvento);
        }
            public bool ActualizarEvento(int id, EventoUpdateDto evento)
            {
            var entidadEvento = new Evento
            {
            IdEvento = id,
            Nombre = evento.Nombre,
            Descripcion = evento.Descripcion,
            FechaInicio = evento.FechaInicio,
            FechaFin = evento.FechaFin,
            Estado = evento.Estado
            };
    return _eventoRepository.Update(id, entidadEvento);
}

        public bool EliminarEvento(int id)
        {
            return _eventoRepository.Delete(id);
        }

        public bool Publicar(int id)
        {
            var evento = _eventoRepository.GetById(id);
            if (evento == null) return false;

            evento.Estado = EstadoEvento.Publicado;
            return _eventoRepository.Update(id, evento);
        }

        public bool Cancelar(int id)
        {
            var evento = _eventoRepository.GetById(id);
            if (evento == null) return false;

            evento.Estado = EstadoEvento.Cancelado;
            return _eventoRepository.Update(id, evento);
        }
    }
}
