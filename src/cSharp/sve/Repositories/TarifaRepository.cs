using Dapper;
using sve.Models;
using sve.Repositories.Contracts;
using System.Data;


namespace sve.Repositories;

public class TarifaRepository : ITarifaRepository
{
    private readonly IDbConnection _connection;

    public TarifaRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Tarifa> GetAll()
    {
        return _connection.Query<Tarifa>("SELECT * FROM Tarifa").ToList();
    }

    public Tarifa? GetById(int id)
    {
        return _connection.QueryFirstOrDefault<Tarifa>(
            "SELECT * FROM Tarifa WHERE IdTarifa = @IdTarifa",
            new { IdTarifa = id });
    }

    public int Add(Tarifa tarifa)
    {
        string sql = @"
                INSERT INTO Tarifa (Precio, Stock, IdSector, IdFuncion)
                VALUES (@Precio, @Stock, @IdSector, @IdFuncion);
                SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, tarifa);
        tarifa.IdTarifa = newId;
        return newId;
    }

    public int Update(Tarifa tarifa)
    {
        string sql = @"
                UPDATE Tarifa 
                SET Precio = @Precio,
                    Stock = @Stock,
                    IdSector = @IdSector,
                    IdFuncion = @IdFuncion
                WHERE IdTarifa = @IdTarifa";
        int rows = _connection.Execute(sql, tarifa);
        return rows > 0 ? tarifa.IdTarifa: 0; 
    }

    public int Delete(int id)
    {
        string sql = "DELETE FROM Tarifa WHERE IdTarifa = @IdTarifa";
        int rows = _connection.Execute(sql, new { IdTarifa = id });
        return rows > 0 ? id : 0; 
    }
}