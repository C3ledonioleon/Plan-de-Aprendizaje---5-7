using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using sve.Models; 

namespace sve.DTOs
{
    public class EventoCreateDto
    {
        public string Nombre { get; set; } 
        public string? Descripcion { get; set; }
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
        public string Nombre { get; set; } 
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoEvento Estado { get; set; }
    }
}

