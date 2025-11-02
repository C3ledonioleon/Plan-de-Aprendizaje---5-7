using sveCore.Models;

namespace sveCore.Servicio.IRepositories;

public interface ITarifaRepository

{
    List<Tarifa> GetAll();
    Tarifa? GetById(int id);
    int Add(Tarifa tarifa);
    int Update(Tarifa tarifa);
    int Delete(int id);

}
