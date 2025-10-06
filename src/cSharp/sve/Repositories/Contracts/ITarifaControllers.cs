using sve.Models;

namespace sve.Repositories.Contracts;

public interface ITarifaRepository

{
    List<Tarifa> GetAll();
    Tarifa? GetById(int id);
    int Add(Tarifa tarifa);
    int Update(Tarifa tarifa);
    int Delete(int id);

}
