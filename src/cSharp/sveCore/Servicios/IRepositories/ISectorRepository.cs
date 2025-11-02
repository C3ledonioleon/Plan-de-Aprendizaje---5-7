using sveCore.Models;

namespace sveCore.Servicio.IRepositories;

public interface ISectorRepository
{
    List<Sector> GetAll();
    Sector? GetById(int id);
    int Add(Sector sector);
    int Update(Sector sector);
    int Delete(int sectorId);
}
