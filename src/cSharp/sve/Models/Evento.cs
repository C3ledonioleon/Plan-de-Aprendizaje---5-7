namespace sve.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public  string Nombre { get; set; } 
        public  string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoEvento Estado { get; set; }
        public List<Funcion>? Funciones { get; set; }
        
    }
    public enum EstadoEvento
    {
        Inactivo = 0,
        Publicado = 1,
        Cancelado = 2
    }

}
