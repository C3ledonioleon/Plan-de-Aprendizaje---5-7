using Dapper;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveDapper.Factories;
using System.Data;


namespace sveDapper.Repositories;

public class TarifaRepository : ITarifaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TarifaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Tarifa> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Tarifa>("SELECT * FROM Tarifa").ToList();
    }

    public Tarifa? GetById(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Tarifa>(
            "SELECT * FROM Tarifa WHERE IdTarifa = @IdTarifa",
            new { IdTarifa = id });
    }

    public int Add(Tarifa tarifa)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                INSERT INTO Tarifa (Precio, Stock, IdSector, IdFuncion, Estado)
                VALUES (@Precio, @Stock, @IdSector, @IdFuncion, @Estado);
                SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, tarifa);
        tarifa.IdTarifa = newId;
        return newId;
    }

    public int Update(Tarifa tarifa)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                UPDATE Tarifa 
                SET Precio = @Precio,
                    Stock = @Stock,
                    IdSector = @IdSector,
                    IdFuncion = @IdFuncion,
                    Estado = @Estado
                WHERE IdTarifa = @IdTarifa";
        int rows = _connection.Execute(sql, tarifa);
        return rows > 0 ? tarifa.IdTarifa: 0; 
    }

    public int Delete(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = "DELETE FROM Tarifa WHERE IdTarifa = @IdTarifa";
        int rows = _connection.Execute(sql, new { IdTarifa = id });
        return rows > 0 ? id : 0; 
    }
}