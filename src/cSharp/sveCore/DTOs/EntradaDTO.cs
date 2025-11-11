using sveCore.Models;
namespace sveCore.DTOs
{
public class EntradaUpdateDto 
    {
    public decimal Precio { get; set; }
    public int IdOrden { get; set; }
    public int IdTarifa { get; set; }
    public int IdCliente { get; set; }   // <-- Agregar
    public int IdFuncion { get; set; }
    public EstadoEntrada Estado { get; set; }
    public object Nombre { get; internal set; }
    }

    public class EntradaDto
    {
        public int IdEntrada { get; set; }
        public decimal Precio { get; set; }
        public int IdOrden { get; set; }
        public int IdTarifa { get; set; }
        public EstadoEntrada Estado { get; set; }
    }
}