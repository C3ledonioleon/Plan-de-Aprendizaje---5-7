using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sve.Models;

namespace sve.DTOs
{
    // DTO para crear una función
    public class FuncionCreateDto
    {
        public int IdEvento { get; set; }
        public int IdLocal { get; set; }
        public DateTime FechaHora { get; set; }
    }

    // DTO para actualizar una función
    public class FuncionUpdateDto : FuncionCreateDto
    {
        public EstadoFuncion Estado { get; set; } // Permite actualizar el estado si fuera necesario
    }

    // DTO de salida (para listar o devolver)
    public class FuncionDto
    {
        public int IdFuncion { get; set; }
        public int IdEvento { get; set; }
        public int IdLocal { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoFuncion Estado { get; set; }
    }
}
