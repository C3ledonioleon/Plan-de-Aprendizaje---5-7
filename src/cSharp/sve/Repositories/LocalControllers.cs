using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class LocalRepository : ILocalRepository
    {
        private readonly SveContext sveContext;

        public LocalRepository(IConfiguration configuration)
        {
    
            this.sveContext = sveContext;
        }

        public List<Local> GetAll()
        {
            return sveContext.Local.ToList();
        }

        public Local? GetById(int id)
        {
            return sveContext.Local.FirstOrDefault(x => x.IdLocal == id);
        }


        public int Add(Local local)
        {
            sveContext.Local.Add(local);
            sveContext.SaveChanges();
            return local.IdLocal; // EF Core genera autom�ticamente el Id
        }

        public bool Update(int id, Local local)
        {
            local.IdLocal = id;
            sveContext.Local.Update(local);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var local = sveContext.Funcion.FirstOrDefault(x => x.IdFuncion == id);
            sveContext.Funcion.Remove(local);
            return sveContext.SaveChanges() > 0;
        }
    }
}
