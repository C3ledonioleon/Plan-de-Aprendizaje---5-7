using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;

namespace sve.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public List<ClienteDto> ObtenerTodo()
    {
        return _clienteRepository.GetAll()
            .Select(cliente => new ClienteDto
            {
                IdCliente = cliente.IdCliente,
                DNI = cliente.DNI,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                IdUsuario = cliente.IdUsuario
            })
            .ToList();
    }

    public Cliente? ObtenerPorId(int id)
    {
        return _clienteRepository.GetById(id);
    }

    public int AgregarCliente(ClienteCreateDto cliente)
    {
        var nuevoCliente = new Cliente
        {
            DNI = cliente.DNI,
            Nombre = cliente.Nombre,
            Telefono = cliente.Telefono,
            IdUsuario = cliente.IdUsuario
        };
        return _clienteRepository.Add(nuevoCliente);
    }

    public int ActualizarCliente(int id, ClienteUpdateDto cliente)
    {
        var clienteExistente = _clienteRepository.GetById(id);
        if (clienteExistente == null) return 0;

        clienteExistente.DNI = cliente.DNI;
        clienteExistente.Nombre = cliente.Nombre;
        clienteExistente.Telefono = cliente.Telefono;
        clienteExistente.IdUsuario = cliente.IdUsuario;

        return _clienteRepository.Update(id, clienteExistente);
    }

    public int EliminarCliente(int id)
    {
        return _clienteRepository.Delete(id);
    }

}
