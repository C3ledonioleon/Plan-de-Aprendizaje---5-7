using sve.Models;

namespace sve.DTOs
{
    public class ClienteCreateDto
    {

        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }
        public int IdUsuario { get; set; }
    }

    public class ClienteUpdateDto : ClienteCreateDto
    {

    }

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }
        public int IdUsuario { get; set; }


        public List<Orden> Ordenes { get; set; }
    }
}


