namespace sve.Models
{
    public class Entrada
    {
        public int IdEntrada { get; set; }
        public decimal Precio { get; set; }
        public EstadoEntrada Estado { get; set; }
        public int IdOrden { get; set; }
        public int IdTarifa { get; set; }
        public int IdCliente { get; set; }
        public int IdFuncion { get; set; }
    }
    public enum EstadoEntrada
    {
        Activa,
        Anulada
    }
}
