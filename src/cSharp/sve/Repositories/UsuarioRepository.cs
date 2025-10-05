using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(IDbConnection connection) : base(connection)
    {
    }

    public Usuario? GetByEmail(string email)
    {
        throw new NotImplementedException();
    }
}
