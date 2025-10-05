using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IEntradaRepository
    {
        List<Entrada> GetAll();
        Entrada? GetById(int id);
        int Add(Entrada entrada); 
        int Update(int id, Entrada entrada);
        int Delete(int id);
    }
}
