using System;
using System.Collections.Generic;

namespace sve.Models
{
    public class Local
    {
        public int IdLocal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int CapacidadTotal { get; set; }
        public List<Sector> Sectores { get; set; } = new();
        public List<Funcion> Funciones { get; set; } = new();
        
        public Local()
        { }
    }
}
