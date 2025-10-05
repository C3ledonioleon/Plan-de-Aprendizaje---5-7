using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IClienteRepository
    {
        List<Cliente> GetAll();
        Cliente? GetById(int id);
        int Add(Cliente cliente);
        int Update(int id, Cliente cliente);
        int Delete(int id);
    }
}