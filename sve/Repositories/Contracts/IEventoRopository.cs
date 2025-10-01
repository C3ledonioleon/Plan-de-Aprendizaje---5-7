using sve.Models;

namespace sve.Repositories.Contracts
{
    public interface IEventoRepository
    {
        List<Evento> GetAll();
        Evento? GetById(int id);
        int Add(Evento evento);
        bool Update(int id, Evento evento);
        bool Delete(int id);
    }
}
