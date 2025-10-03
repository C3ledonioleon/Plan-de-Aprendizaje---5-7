
using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IUsuarioRepository
    {
        List<Usuario> GetAll();
        Usuario? GetById(int id);
        Usuario? GetByEmail(string email);
        int Add(Usuario usuario);
        bool Update(Usuario usuario);
        bool Delete(int id);
    }
}
