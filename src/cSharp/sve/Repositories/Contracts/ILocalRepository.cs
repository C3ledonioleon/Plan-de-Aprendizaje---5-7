using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface ILocalRepository
    {
        List<Local> GetAll();
        Local? GetById(int id);
        int Add(Local local);
        bool Update(int id, Local local);
        bool Delete(int id);
    }
}
