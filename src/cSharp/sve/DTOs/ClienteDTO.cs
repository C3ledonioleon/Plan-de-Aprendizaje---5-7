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
        // si después querés permitir actualizar también las ordenes,
        // podés agregar la lista acá, de momento lo dejamos igual
    }

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }

        // Podrías devolver también ordenes si lo necesitás en respuestas
        public List<Orden> Ordenes { get; set; }
    }
}


