using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class EntradaRepository : IEntradaRepository
    {
   
        private readonly SveContext sveContext;

        public EntradaRepository(SveContext sveContext)
        {
              this .sveContext = sveContext;
        }

        public List<Entrada> GetAll()
        {
            return sveContext.Entrada.ToList();
        }

        public Entrada? GetById(int id)
        {
            return sveContext.Entrada.FirstOrDefault(x => x.IdEntrada == id);
        }

        public int Add(Entrada entrada)
        {
            sveContext.Entrada.Add(entrada);
            sveContext.SaveChanges();
            return entrada.IdEntrada;
        }

        public bool Update(int id, Entrada entrada)
        {
            entrada.IdEntrada = id;
            sveContext.Entrada.Update(entrada);
            return sveContext.SaveChanges() > 0;
        }
        public bool Delete(int id)
        {
            var entrada = sveContext.Entrada.FirstOrDefault(x => x.IdEntrada == id);
            sveContext.Entrada.Remove(entrada);
            return sveContext.SaveChanges() > 0;
        }
    }
}
        