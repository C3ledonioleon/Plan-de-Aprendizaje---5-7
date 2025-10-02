using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class SectorRepository : ISectorRepository
    {
        private readonly SveContext sveContext;

        public SectorRepository(IConfiguration configuration)
        {
            sveContext = sveContext;
        }

        public List<Sector> GetAll()
        {
            return sveContext.Sector.ToList();
        }

        public Sector? GetById(int id)
        {
            return sveContext.Sector.FirstOrDefault(x => x.IdSector == id);
        }

        public int Add(Sector sector)
        {
            sveContext.Sector.Add(sector);
            sveContext.SaveChanges();
            return sector.IdSector; // EF Core genera autom�ticamente el Id
        }

        public bool Update(int id, Sector sector)
        {
            sector.IdSector = id;
            sveContext.Sector.Update(sector);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var sector = sveContext.Sector.FirstOrDefault(x => x.IdSector == id);
            sveContext.Sector.Remove(sector);
            return sveContext.SaveChanges() > 0;
        }
    }
}
