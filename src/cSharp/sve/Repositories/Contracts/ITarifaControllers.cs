using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface ITarifaRepository

    {
        List<Tarifa> GetAll(); // GET /tarifas
        Tarifa? GetById(int id); // GET /tarifas/{id}
        int Add(Tarifa tarifa); // POST /tarifas
        bool Update(int id, Tarifa tarifa); // PUT /tarifas/{id}
        bool Delete(int id);

    }
}
