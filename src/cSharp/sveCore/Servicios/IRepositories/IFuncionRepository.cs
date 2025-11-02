using sveCore.Models;
namespace sveCore.Servicio.IRepositories;

public interface IFuncionRepository
{
    List<Funcion> GetAll();
    Funcion? GetById(int id);
    int Add(Funcion funcion);
    int Update(Funcion funcion);
    int Delete(int id);
}
