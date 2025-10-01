using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts;

    public interface IEventoService
    {
        List<Evento> ObtenerTodo();
        Evento? ObtenerPorId(int id);
        int AgregarEvento(EventoCreateDto evento);
        bool ActualizarEvento(int id, EventoUpdateDto evento );
        bool EliminarEvento(int id);
        bool Publicar(int id);
        bool Cancelar(int id);
    }
