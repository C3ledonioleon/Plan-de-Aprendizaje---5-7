using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface ILocalRepository
    {
        List<Local> GetAll();
        Local? GetById(int id);
        int Add(Local local);
        int Update(int id, Local local);
        int Delete(int id);
    }
}
