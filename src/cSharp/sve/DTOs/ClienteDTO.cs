using sve.Models;

namespace sve.DTOs
{
    public class ClienteCreateDto
    {

        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }
    }

    public class ClienteUpdateDto : ClienteCreateDto
    {
        // si despu�s quer�s permitir actualizar tambi�n las ordenes,
        // pod�s agregar la lista ac�, de momento lo dejamos igual
    }

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }

        // Podr�as devolver tambi�n ordenes si lo necesit�s en respuestas
        public List<Orden> Ordenes { get; set; }
    }
}


