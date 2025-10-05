using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IOrdenRepository
    {
        List<Orden> GetAll();
        Orden? GetById(int id);
        int Add(Orden orden);
        int Update(int id, Orden orden);
        int Delete (int id);
    }
}
