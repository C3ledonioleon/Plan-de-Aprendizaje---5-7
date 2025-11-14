using sveCore.Models;
using sveCore.Servicio.IRepositories;
using Dapper;
using sveDapper.Factories;

namespace sveDapper.Repositories;

public class LocalRepository : ILocalRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public LocalRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Local> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Local>("SELECT * FROM Local").ToList();
    }

    public Local? GetById(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Local>(
            "SELECT * FROM Local WHERE IdLocal = @IdLocal",
            new { IdLocal = id });
    }

    public int Add(Local local)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                INSERT INTO Local (Nombre, Direccion, CapacidadTotal)
                VALUES (@Nombre, @Direccion, @CapacidadTotal);
                SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, local);
        local.IdLocal = newId;
        return newId;
    }

    public int Update(Local local)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                UPDATE Local 
                SET Nombre = @Nombre,
                    Direccion = @Direccion, 
                    CapacidadTotal = @CapacidadTotal
                WHERE IdLocal = @IdLocal";
        int rows = _connection.Execute(sql, local);
        return rows > 0 ? local.IdLocal : 0;
    }

    public int Delete(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = "DELETE FROM Local WHERE IdLocal = @IdLocal";
        int rows = _connection.Execute(sql, new { IdLocal = id });
        return rows  > 0 ? id : 0;
    }
}