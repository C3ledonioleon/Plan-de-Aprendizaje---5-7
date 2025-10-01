using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using sve.Models; 

namespace sve.DTOs;

public class ClienteCreateDto
{
    public string DNI { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
}

public class ClienteUpdateDto : ClienteCreateDto
{
}

public class ClienteDto
{
    public int IdCliente { get; set; }
    public string DNI { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
}

