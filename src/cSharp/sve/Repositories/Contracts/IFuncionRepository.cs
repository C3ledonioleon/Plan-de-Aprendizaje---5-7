using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IFuncionRepository
    {
            List<Funcion> GetAll();
            Funcion? GetById(int id);
            int Add(Funcion funcion);
            int Update( int id ,Funcion funcion);
            int Delete (int id);
    }
}
