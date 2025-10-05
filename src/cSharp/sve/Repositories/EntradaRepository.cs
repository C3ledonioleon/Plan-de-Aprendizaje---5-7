using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class EntradaRepository : BaseRepository<Entrada>, IEntradaRepository
{
    public EntradaRepository(IDbConnection connection) : base(connection)
    {
    }
}
