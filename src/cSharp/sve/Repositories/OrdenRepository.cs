using sve.Models;
using sve.Repositories.Contracts;
using System.Data;

namespace sve.Repositories;

public class OrdenRepository : BaseRepository<Orden>, IOrdenRepository
{
    public OrdenRepository(IDbConnection connection) : base(connection)
    {
    }
}
