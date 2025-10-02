using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly SveContext sveContext;

        public OrdenRepository(IConfiguration configuration)
        {
            this.sveContext = sveContext;
        }


        public List<Orden> GetAll()
        {
            return sveContext.Orden.ToList();
        }

        public Orden? GetById(int id)
        {
            return sveContext.Orden.FirstOrDefault(x => x.IdOrden == id);
        }
        public int Add(Orden orden)
        {
            sveContext.Orden.Add(orden);
            sveContext.SaveChanges();
            return orden.IdOrden; // EF Core genera autom�ticamente el Id
        }

        public bool Update(int id, Orden orden)
        {
            orden.IdOrden = id;
            sveContext.Orden.Update(orden);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var orden = sveContext.Orden.FirstOrDefault(x => x.IdOrden == id);
            sveContext.Orden.Remove(orden);
            return sveContext.SaveChanges() > 0;
        }
    }
}
