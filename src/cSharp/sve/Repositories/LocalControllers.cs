using sve.Models;
using sve.Repositories.Contracts;
using System.Data;
using Dapper;

namespace sve.Repositories;

public class LocalRepository : ILocalRepository
{
    private readonly IDbConnection _connection;

    public LocalRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Local> GetAll()
    {
        return _connection.Query<Local>("SELECT * FROM Local").ToList();
    }

    public Local? GetById(int id)
    {

        return _connection.QueryFirstOrDefault<Local>(
            "SELECT * FROM Local WHERE IdLocal = @IdLocal",
            new { IdLocal = id });
    }

    public int Add(Local local)
    {

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
        string sql = @"
                UPDATE Local 
                SET Nombre = @Nombre,
                    Direccion = @Direccion, 
                    CapacidadTotal = @CapacidadTotal,
                WHERE IdLocal = @IdLocal";
        int rows = _connection.Execute(sql, local);
        return rows > 0 ? local.IdLocal : 0;
    }

    public int Delete(int id)
    {
        string sql = "DELETE FROM Local WHERE IdLocal = @IdLocal";
        int rows = _connection.Execute(sql, new { IdLocal = id });
        return rows  > 0 ? id : 0;
    }
}