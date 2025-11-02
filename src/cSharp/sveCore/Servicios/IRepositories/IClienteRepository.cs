using sveCore.Models;

namespace sveCore.Servicio.IRepositories;

public interface IClienteRepository
{
    List<Cliente> GetAll();
    Cliente? GetById(int id);
    int Add(Cliente cliente);
    int Update(int id, Cliente cliente);
    int Delete(int id);
}