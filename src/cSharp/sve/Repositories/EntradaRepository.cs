using sve.Models;
using sve.Repositories.Contracts;
using System.Data;
using Dapper;

namespace sve.Repositories;

public class EntradaRepository : IEntradaRepository
{
    private readonly IDbConnection _connection;

    public EntradaRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Entrada> GetAll()
    {
        return _connection.Query<Entrada>("SELECT * FROM Entrada").ToList();
    }

    public Entrada? GetById(int id)
    {
        return _connection.QueryFirstOrDefault<Entrada>(
            "SELECT * FROM Entrada WHERE IdEntrada = @IdEntrada",
            new { IdEntrada = id });
    }

    public int Add(Entrada entrada)
    {
 
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
        string sql = "DELETE FROM Entrada WHERE IdEntrada = @IdEntrada";
        int rows = _connection.Execute(sql, new { IdEntrada = id });
        return rows > 0 ? id : 0;
    }
}
    