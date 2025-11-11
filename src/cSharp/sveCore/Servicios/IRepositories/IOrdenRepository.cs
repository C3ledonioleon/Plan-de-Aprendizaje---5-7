using System.Reflection.Metadata;
using sveCore.Models;

namespace sveCore.Servicio.IRepositories;

public interface IOrdenRepository
{
    List<Orden> GetAll();
    Orden? GetById(int id);
    int Add(Orden orden);
    int Update(Orden orden);
    int Delete(int id);
    bool PagarOrden(int idOrden);
    bool CancelarOrden(int idOrden);
}
