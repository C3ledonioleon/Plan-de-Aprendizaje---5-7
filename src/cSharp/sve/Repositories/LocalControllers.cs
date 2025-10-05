using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class LocalRepository : BaseRepository<Local>, ILocalRepository
{
    public LocalRepository(IDbConnection connection) : base(connection)
    {
    }
}
