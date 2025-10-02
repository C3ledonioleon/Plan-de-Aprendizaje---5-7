using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class FuncionRepository : IFuncionRepository
    {
        private readonly SveContext sveContext;

        public FuncionRepository(SveContext sveContext)
        {
            this.sveContext = sveContext;
        }

        public List<Funcion> GetAll()
        {
            return sveContext.Funcion.ToList();
        }


        public Funcion? GetById(int id)
        {
            return sveContext.Funcion.FirstOrDefault(x => x.IdFuncion == id);
        }

        public int Add(Funcion funcion)
        {
            sveContext.Funcion.Add(funcion);
            sveContext.SaveChanges();
            return funcion.IdFuncion; // EF Core genera automáticamente el Id
        }


        public bool Update(int id, Funcion funcion)
        {
            funcion.IdFuncion = id;
            sveContext.Funcion.Update(funcion);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var funcion = sveContext.Funcion.FirstOrDefault(x => x.IdFuncion == id);
            sveContext.Funcion.Remove(funcion);
            return sveContext.SaveChanges() > 0;
        }
    }
}
