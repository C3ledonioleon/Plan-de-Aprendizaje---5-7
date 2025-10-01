namespace sve.Models
{
    public class Entrada
    {
        public int IdEntrada { get; set; }
        public decimal Precio { get; set; }
        public int IdOrden { get; set; }
        public int IdTarifa { get; set; }
        public EstadoEntrada Estado { get; set; }
        
        public Entrada()
        { }
    }

    public enum EstadoEntrada
    {
        Activa,
        Anulada
    }
}
