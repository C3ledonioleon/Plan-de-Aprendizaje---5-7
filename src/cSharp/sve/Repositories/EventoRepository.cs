using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;
using System.Data;

namespace sve.Repositories;

public class EventoRepository : BaseRepository<Evento>, IEventoRepository
{
    public EventoRepository(IDbConnection connection) : base(connection)
    {
    }
}
