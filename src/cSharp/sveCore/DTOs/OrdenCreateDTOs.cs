using System;
using System.Collections.Generic;
using sveCore.Models;

namespace sveCore.DTOs
{
    public class OrdenCreateDto
    {
        
        public int IdTarifa { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class OrdenUpdateDto : OrdenCreateDto
    {
        public EstadoOrden Estado { get; set; }
    }

    public class OrdenDto
    {
        public int IdOrden { get; set; }
        public int IdTarifa { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoOrden Estado { get; set; }
        public Cliente Cliente { get; set; }
        public Tarifa Tarifa { get; set; }
        public List<Entrada> Entradas { get; set; }
    }
}
