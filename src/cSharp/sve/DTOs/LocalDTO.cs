namespace sve.DTOs;

public class LocalCreateDto
{
    public  string Nombre { get; set; } 
    public  string Direccion { get; set; } 
    public int CapacidadTotal { get; set; }
}

public class LocalUpdateDto : LocalCreateDto
{

}

public class LocalDto
{
    public int IdLocal { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public int CapacidadTotal { get; set; }
    public List<SectorDto> Sectores { get; set; } = new();
    public List<FuncionDto> Funciones { get; set; } = new();
}
