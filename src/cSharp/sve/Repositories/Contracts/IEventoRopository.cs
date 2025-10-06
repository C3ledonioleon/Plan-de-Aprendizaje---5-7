using sve.Models;

namespace sve.Repositories.Contracts;

public interface IEventoRepository
{
    List<Evento> GetAll();
    Evento? GetById(int id);
    int Add(Evento evento);
    int Update(Evento evento);
    int Delete(int id);
}
