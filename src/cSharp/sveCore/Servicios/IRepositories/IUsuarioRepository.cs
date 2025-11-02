using sveCore.Models;
namespace sveCore.Servicio.IRepositories;

public interface IUsuarioRepository
{
    List<Usuario> GetAll();
    Usuario? GetById(int id);
    Usuario? GetByEmail(string email);
    int Add(Usuario usuario);
    int Update(Usuario usuario);
    int Delete(int id);
    Usuario? GetByRefreshToken(string refreshToken);
    void UpdateRefreshToken(string email, string refreshToken, DateTime expiracion);
}
