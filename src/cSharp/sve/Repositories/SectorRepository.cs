using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class SectorRepository : BaseRepository<Sector>, ISectorRepository
{
    public SectorRepository(IDbConnection connection) : base(connection)
    {
    }
}
