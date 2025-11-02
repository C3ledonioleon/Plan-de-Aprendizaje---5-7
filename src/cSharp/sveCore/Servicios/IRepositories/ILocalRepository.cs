using sveCore.Models;
namespace sveCore.Servicio.IRepositories;

public interface ILocalRepository
{
    List<Local> GetAll();
    Local? GetById(int id);
    int Add(Local local);
    int Update(Local local);
    int Delete(int id);
}
