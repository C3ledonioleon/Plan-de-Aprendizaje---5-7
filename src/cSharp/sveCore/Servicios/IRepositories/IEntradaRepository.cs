using sveCore.Models;

namespace sveCore.Servicio.IRepositories;

public interface IEntradaRepository
{
    List<Entrada> GetAll();
    Entrada? GetById(int id);
    int Add(Entrada entrada); 
    int Update(Entrada entrada);
    int Delete(int id);
}
