using sve.Models;

namespace sve.DTOs
{
    public class EventoCreateDto
    {
        public required string Nombre { get; set; } 
        public required string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoEvento Estado { get; set; }
    }

    public class EventoUpdateDto : EventoCreateDto
    { 
    }

    public class EventoDto 
    {
        public int IdEvento { get; set; }
        public required string Nombre { get; set; } 
        public required string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoEvento Estado { get; set; }
    }
}

