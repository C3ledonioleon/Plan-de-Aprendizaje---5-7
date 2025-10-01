namespace sve.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string DNI { get; set; }         
        public string Nombre { get; set; }     
        public string Telefono { get; set; }
        public List<Orden> Ordenes { get; set; }
        
        public Cliente()
        { }
    }
    
}

