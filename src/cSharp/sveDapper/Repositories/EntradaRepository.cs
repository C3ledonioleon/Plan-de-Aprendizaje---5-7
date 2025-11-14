using sveCore.Models;
using sveCore.Servicio.IRepositories;
using System.Data;
using Dapper;
using sveDapper.Factories;

namespace sveDapper.Repositories;

public class EntradaRepository : IEntradaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EntradaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public List<Entrada> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Entrada>("SELECT * FROM Entrada").ToList();
    }

    public Entrada? GetById(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Entrada>(
            "SELECT * FROM Entrada WHERE IdEntrada = @IdEntrada",
            new { IdEntrada = id });
    }

    public int Add(Entrada entrada)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                INSERT INTO Entrada (Precio, IdCliente, IdFuncion,IdTarifa, IdOrden, Estado )
                VALUES (@Precio, @IdCliente, @IdFuncion, @IdTarifa, @IdOrden, @Estado );
                SELECT LAST_INSERT_ID();";

        int newId = _connection.ExecuteScalar<int>(sql, entrada);
        entrada.IdEntrada = newId;
        return newId;
    }

    public int Update(Entrada entrada)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                UPDATE Entrada 
                SET Precio = @Precio,
                IdOrden = @IdOrden,
                IdTarifa = @IdTarifa,
                IdFuncion = @IdFuncion,
                IdCliente = @IdCliente ,
                Estado = @Estado
                WHERE IdEntrada = @IdEntrada";
        int rows = _connection.Execute(sql, entrada);
        return rows > 0 ? entrada.IdEntrada: 0;
    }

    public int Delete(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = "DELETE FROM Entrada WHERE IdEntrada = @IdEntrada";
        int rows = _connection.Execute(sql, new { IdEntrada = id });
        return rows > 0 ? id : 0;
    }
}
    