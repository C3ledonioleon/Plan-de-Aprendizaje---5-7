namespace sve.Models
{
    public class Orden
    {
        public int IdOrden { get; set; }
        public int IdTarifa { get; set; }  // FK hacia Tarifa
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoOrden Estado { get; set; }
        public Cliente Cliente { get; set; }
        public Tarifa Tarifa { get; set; }
        public List<Entrada> Entradas { get; set; }

        public Orden()
      { }

    }

    public enum EstadoOrden
    {
        Creada = 0,
        Pagada = 1,
        Cancelada = 2
    }
}
