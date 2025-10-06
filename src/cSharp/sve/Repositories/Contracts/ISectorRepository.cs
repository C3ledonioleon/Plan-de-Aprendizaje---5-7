using sve.Models;

namespace sve.Repositories.Contracts;

public interface ISectorRepository
{
    List<Sector> GetAll();
    Sector? GetById(int id);
    int Add(Sector sector);
    int Update(Sector sector);
    int Delete(int sectorId);
}
