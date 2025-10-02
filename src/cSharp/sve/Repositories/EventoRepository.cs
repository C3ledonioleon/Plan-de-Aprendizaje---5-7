using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly SveContext  sveContext;

        public EventoRepository(SveContext sveContext)
        {
            this.sveContext = sveContext;
        }

        public List<Evento> GetAll()
        {
            return sveContext.Evento.ToList();
        }

        public Evento? GetById(int id)
        {
            return sveContext.Evento.FirstOrDefault(x => x.IdEvento == id);
        }

        public int Add(Evento evento)
        {
            sveContext.Evento.Add(evento);
            sveContext.SaveChanges();
            return evento.IdEvento;
        }

        public bool Update(int id,Evento evento)
        {
            evento.IdEvento = id;
            sveContext.Evento.Update(evento);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var evento = sveContext.Evento.FirstOrDefault(x => x.IdEvento == id);
            sveContext.Evento.Remove(evento);
            return sveContext.SaveChanges() > 0;
        }

    }
}