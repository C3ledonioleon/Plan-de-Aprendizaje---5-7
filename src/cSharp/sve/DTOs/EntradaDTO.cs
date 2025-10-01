using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using sve.Models; 

namespace sve.DTOs;

public class EntradaCreateDto
{
    public decimal Precio { get; set; }
    public int IdOrden { get; set; }
    public int IdTarifa { get; set; }
}

public class EntradaUpdateDto : EntradaCreateDto
{
    public EstadoEntrada Estado { get; set; } // Permite actualizar el estado si fuera necesario
}

public class EntradaDto
{
    public int IdEntrada { get; set; }
    public decimal Precio { get; set; }
    public int IdOrden { get; set; }
    public int IdTarifa { get; set; }
    public EstadoEntrada Estado { get; set; }
}