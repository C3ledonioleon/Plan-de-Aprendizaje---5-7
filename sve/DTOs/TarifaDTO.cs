using sve.Models;
using System.Collections.Generic;

namespace sve.DTOs
{
    // DTO para crear una Tarifa
    public class TarifaCreateDto
    {
        public int IdFuncion { get; set; }
        public int IdSector { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public EstadoTarifa Estado { get; set; } = EstadoTarifa.Activa;
    }

    // DTO para actualizar una Tarifa
    public class TarifaUpdateDto
    {
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public EstadoTarifa Estado { get; set; }
    }

    // DTO para devolver información completa de una Tarifa
    public class TarifaDto
    {
        public int IdTarifa { get; set; }
        public int IdFuncion { get; set; }
        public int IdSector { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public EstadoTarifa Estado { get; set; }
        public Funcion? Funcion { get; set; }
        public Sector? Sector { get; set; }
        public List<Entrada> Entradas { get; set; } = new();
    }
}
