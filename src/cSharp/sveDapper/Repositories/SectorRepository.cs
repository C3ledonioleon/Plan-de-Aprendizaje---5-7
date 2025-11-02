using Dapper;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using System.Data;

namespace sveDapper.Repositories;

public class SectorRepository : ISectorRepository
{
    private readonly IDbConnection _connection;

    public SectorRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Sector> GetAll()
    {

        return _connection.Query<Sector>("SELECT * FROM Sector").ToList();
    }

    public Sector? GetById(int id)
    {
        return _connection.QueryFirstOrDefault<Sector>(
            "SELECT * FROM Sector WHERE IdSector = @IdSector",
            new { IdSector = id });
    }

    public int Add(Sector sector)
    {

        string sql = @"
                INSERT INTO Sector (Nombre, Capacidad, IdLocal)
                VALUES (@Nombre, @Capacidad, @IdLocal);
                SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, sector);
        sector.IdSector = newId;
        return newId;
    }

    public int Update(Sector sector)
    {
        string sql = @"
                UPDATE Sector 
                SET Nombre = @Nombre,
                Capacidad = @Capacidad,
                IdLocal = @IdLocal
                WHERE IdSector = @IdSector";
        int rows = _connection.Execute(sql, sector);
        return rows > 0 ? sector.IdSector : 0; 
    }

    public int Delete(int id)
    {
        string sql = "DELETE FROM Sector WHERE IdSector = @IdSector";
        int rows = _connection.Execute(sql, new { IdSector = id });
        return rows > 0 ? id : 0; 
    }
}