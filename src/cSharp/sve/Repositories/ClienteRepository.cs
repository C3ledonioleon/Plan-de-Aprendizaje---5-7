using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(IDbConnection connection) : base(connection)
    {
    }
}
