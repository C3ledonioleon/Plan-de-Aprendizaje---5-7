using sve.Models;

namespace sve.DTOs
{
    public class EventoCreateDto
    {
        public required string Nombre { get; set; } 
        public required string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class EventoUpdateDto : EventoCreateDto
    {
        public EstadoEvento Estado { get; set; } // Permite actualizar el estado si fuera necesario
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

