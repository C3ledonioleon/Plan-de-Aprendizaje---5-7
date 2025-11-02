namespace sve.Entidades;
public class QR
{
    public int IdQR { get; set; }
    public string Codigo { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public TimeSpan Duracion { get; set; }
    public string VCard { get; set; }
    public string URL { get; set; }

}
