using System;

namespace sveCore.Models
{
    public class Funcion
    {
        public int IdFuncion { get; set; }
        public int IdEvento { get; set; }
        public int IdLocal { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoFuncion Estado { get; set; }
        public Evento Evento { get; set; }
        public Local Local { get; set; }
        public List<Tarifa> Tarifas { get; set; }
        

    }
    public enum EstadoFuncion
    {
        Pendiente = 0,  
        Cancelada = 1,   
        Finalizada = 2  
    }
}
