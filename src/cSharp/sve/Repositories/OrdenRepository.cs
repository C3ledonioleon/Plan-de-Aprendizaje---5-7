using Dapper;
using sve.Models;
using sve.Repositories.Contracts;
using System.Data;

namespace sve.Repositories;

public class OrdenRepository : IOrdenRepository
{
    private readonly IDbConnection _connection;

    public OrdenRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Orden> GetAll()
    {

        return _connection.Query<Orden>("SELECT * FROM Orden").ToList();
    }

    public Orden? GetById(int id)
    {
        return _connection.QueryFirstOrDefault<Orden>(
            "SELECT * FROM Orden WHERE IdOrden = @IdOrden",
            new { IdOrden = id });
    }

    public int Add(Orden orden)
    {
        string sql = @"
                INSERT INTO Orden (Total, Fecha, IdCliente, IdTarifa, Estado)
                VALUES (@Total, @Fecha, @IdCliente, @IdTarifa, @Estado);
                SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, orden);
        orden.IdOrden = newId;
        return newId;
    }

    public int Update(Orden orden)
    {
        string sql = @"
                UPDATE Orden 
                SET Total = @Total, 
                    Fecha = @Fecha,
                    Estado = @Estado,
                    IdCliente = @IdCliente,
                    IdTarifa = @IdTarifa
                WHERE IdOrden = @IdOrden";
        int rows = _connection.Execute(sql, orden);
        return rows > 0 ? orden.IdOrden : 0;
    }

    public int Delete(int id)
    {
        string sql = "DELETE FROM Orden WHERE IdOrden = @IdOrden";
        int rows = _connection.Execute(sql, new { IdOrden = id });
        return rows > 0 ? id : 0; 
    }
}