using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class TarifaRepository : BaseRepository<Tarifa>, ITarifaRepository
{
    public TarifaRepository(IDbConnection connection) : base(connection)
    {
    }
}
