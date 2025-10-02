using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sve.Models
{
    public class Usuario
    {
        public int IdUsuario {get; set;}
        public required string Apodo {get; set;}
        public required string Email {get; set; }
        public string contrasenia {get; set; }
        public RolUsuario Rol {get; set; }
        public Usuario() 
        { }

    }
    public enum RolUsuario
    {
        Administrador,
        Empleado,
        Cliente
    }
}