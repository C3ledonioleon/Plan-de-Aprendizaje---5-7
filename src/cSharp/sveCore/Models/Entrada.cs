namespace sveCore.Models;

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
        Activa = 0,   // Todavía no se ha usado
        Usado = 1,    // Ya se utilizó
        Vencido = 2,  // Venció la entrada
        Anulada = 3 , // Se anuló   // Se anuló  
    }


