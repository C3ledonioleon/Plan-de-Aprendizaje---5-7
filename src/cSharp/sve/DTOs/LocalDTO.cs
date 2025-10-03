using System;
using System.Collections.Generic;
using sve.Models;

namespace sve.DTOs
{
    // DTO para crear un Local
    public class LocalCreateDto
    {
        public  string Nombre { get; set; } 
        public  string Direccion { get; set; } 
        public int CapacidadTotal { get; set; }
    }

    // DTO para actualizar un Local
    public class LocalUpdateDto : LocalCreateDto
    {
        // Podés agregar propiedades adicionales si querés validar cambios específicos
    }

    // DTO de salida
    public class LocalDto
    {
        public int IdLocal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int CapacidadTotal { get; set; }
        public List<SectorDto> Sectores { get; set; } = new();
        public List<FuncionDto> Funciones { get; set; } = new();
    }
}
