using sve.Models;
using System.Collections.Generic;

namespace sve.DTOs
{
    public class SectorCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public int IdLocal { get; set; }
    }

    public class SectorUpdateDto : SectorCreateDto
    {
        // Permite actualizar algún estado o propiedad extra si fuera necesario
    }

    public class SectorDto
    {
        public int IdSector { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public int IdLocal { get; set; }
        public Local Local { get; set; }
        public List<Tarifa> Tarifas { get; set; } = new();
    }
}
