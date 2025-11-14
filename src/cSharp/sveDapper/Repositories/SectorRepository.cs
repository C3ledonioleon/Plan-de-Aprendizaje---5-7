using Dapper;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveDapper.Factories;
using System.Data;

namespace sveDapper.Repositories;

public class SectorRepository : ISectorRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SectorRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Sector> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Sector>("SELECT * FROM Sector").ToList();
    }

    public Sector? GetById(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Sector>(
            "SELECT * FROM Sector WHERE IdSector = @IdSector",
            new { IdSector = id });
    }

    public int Add(Sector sector)
    {
        using var _connection = _connectionFactory.CreateConnection();
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
        using var _connection = _connectionFactory.CreateConnection();
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
        using var _connection = _connectionFactory.CreateConnection();
        string sql = "DELETE FROM Sector WHERE IdSector = @IdSector";
        int rows = _connection.Execute(sql, new { IdSector = id });
        return rows > 0 ? id : 0; 
    }
}