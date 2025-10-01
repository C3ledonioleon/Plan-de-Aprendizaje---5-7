using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IFuncionRepository
    {
            List<Funcion> GetAll();
            Funcion? GetById(int id);
            int Add(Funcion funcion);
            bool Update( int id ,Funcion funcion);
            bool Delete (int id);
    }
}
