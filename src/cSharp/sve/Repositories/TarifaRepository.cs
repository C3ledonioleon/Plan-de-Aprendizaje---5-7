using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
        private readonly SveContext sveContext;

        public TarifaRepository(IConfiguration configuration)
        {
            sveContext = sveContext;
        }

        public List<Tarifa> GetAll()
        {
            return sveContext.Tarifa.ToList();
        }

        public Tarifa? GetById(int id)
        {
            return sveContext.Tarifa.FirstOrDefault(x => x.IdTarifa == id);
        }

        public int Add(Tarifa tarifa)
        {
            sveContext.Tarifa.Add(tarifa);
            sveContext.SaveChanges();
            return tarifa.IdTarifa; // EF Core genera autom�ticamente el Id
        }

        public bool Update(int id, Tarifa tarifa)
        {
            tarifa.IdTarifa = id;
            sveContext.Tarifa.Update(tarifa);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var tarifa = sveContext.Tarifa.FirstOrDefault(x => x.IdTarifa == id);
            sveContext.Tarifa.Remove(tarifa);
            return sveContext.SaveChanges() > 0;
        }
    }
}
