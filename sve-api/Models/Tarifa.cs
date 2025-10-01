namespace sve.Models
{
    public class Tarifa
    {
        public int IdTarifa { get; set; }
        public int  IdFuncion { get; set; }
        public int IdSector { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public Funcion? Funcion { get; set; }
        public Sector? Sector { get; set; }
        public EstadoTarifa Estado { get; set; }
        public List<Entrada> Entradas { get; set; } 
        public Tarifa()
        { }
    }

    public enum EstadoTarifa
    {
        Activa,
        Inactiva
    }
}
