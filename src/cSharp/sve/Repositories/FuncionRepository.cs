using sve.Models;
using sve.Repositories.Contracts;
using System.Data;

namespace sve.Repositories;

public class FuncionRepository : BaseRepository<Funcion>, IFuncionRepository
{
    public FuncionRepository(IDbConnection connection) : base(connection)
    {
    }
}
