using sveCore.DTOs;
using sveCore.Models;

namespace sveCore.Services.IServices;
    public interface IEventoService
    {
        List<EventoDto> ObtenerTodo();
        Evento? ObtenerPorId(int id);
        int AgregarEvento(EventoCreateDto evento);
        int ActualizarEvento(int id, EventoUpdateDto evento );
        int EliminarEvento(int id);
        int Publicar(int id);
        int Cancelar(int id);
    }
