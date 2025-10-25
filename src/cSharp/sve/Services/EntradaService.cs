using System.Text.RegularExpressions;
using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services;

public class EntradaService : IEntradaService
{
    private readonly IEntradaRepository _entradaRepository;

    public EntradaService(IEntradaRepository entradaRepository)
    {
        _entradaRepository = entradaRepository;
    }

    public List<EntradaDto> ObtenerTodo()
    {
        return _entradaRepository.GetAll()
            .Select(entrada => new EntradaDto
            {
                IdEntrada = entrada.IdEntrada,
                Precio = entrada.Precio,
                IdOrden = entrada.IdOrden,
                IdTarifa = entrada.IdTarifa,
                Estado = entrada.Estado
            }).ToList();
    }

    public Entrada? ObtenerPorId(int id)
    {
        return _entradaRepository.GetById(id);
    }

    public int AgregarEntrada(EntradaCreateDto entrada)
    {
        var nuevaEntrada = new Entrada
        {
            Precio = entrada.Precio,
            IdOrden = entrada.IdOrden,
            IdTarifa = entrada.IdTarifa,
            Estado = EstadoEntrada.Activa,
            IdCliente = entrada.IdCliente, 
            IdFuncion = entrada.IdFuncion
        };
        return _entradaRepository.Add( nuevaEntrada);
    }
    public int ActualizarEntrada(int id, EntradaUpdateDto entrada)
    {
         var existente = _entradaRepository.GetById(id);
        if (existente == null) return 0;

        existente.Precio = entrada.Precio;
        existente.IdOrden = entrada.IdOrden;
        existente.IdTarifa = entrada.IdTarifa;
        existente.Estado = entrada.Estado;
        
        return _entradaRepository.Update(existente);
    }

    public bool AnularEntrada(int id)
    {
        var entrada = _entradaRepository.GetById(id);
        if (entrada == null) return false;

        entrada.Estado = EstadoEntrada.Anulada;
        return _entradaRepository.Update(entrada) > 0;
    }

    public int EliminarEntrada(int id)
    {
        return _entradaRepository.Delete(id);
    }
// VALIDA UN QR ESCANEADO
public string ValidarQR(string contenido)
{
    // Buscar el número que aparece después de "/entradas/"
    var match = Regex.Match(contenido, @"/entradas/(\d+)/qr");
    if (!match.Success)
        return "FirmaInvalida";

    int entradaId = int.Parse(match.Groups[1].Value);

    var entrada = _entradaRepository.GetById(entradaId);
    if (entrada == null)
        return "NoExiste";

    switch (entrada.Estado)
    {
        case EstadoEntrada.Usado: return "Usado";
        case EstadoEntrada.Vencido: return "Vencido";
        case EstadoEntrada.Anulada: return "Anulada";
        default:
            entrada.Estado = EstadoEntrada.Usado;
            _entradaRepository.Update(entrada);
            return "Activa";
    }
}
    
}
