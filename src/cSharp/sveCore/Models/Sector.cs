namespace sveCore.Models
{
    public class Sector
    {
        public int IdSector { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public int IdLocal { get; set; }
        public Local Local { get; set; }
        public List<Tarifa> Tarifas { get; set; }
    }

    
}

