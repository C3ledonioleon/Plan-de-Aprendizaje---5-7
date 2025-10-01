using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IClienteRepository
    {
        List<Cliente> GetAll();
        Cliente? GetById(int id);
        int Add(Cliente cliente);
        bool Update(int id, Cliente cliente);
        bool Delete(int id);
    }
}