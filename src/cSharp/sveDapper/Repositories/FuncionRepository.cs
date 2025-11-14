using sveCore.Models;
using sveCore.Servicio.IRepositories;
using Dapper;
using sveDapper.Factories;

namespace sveDapper.Repositories;

public class FuncionRepository : IFuncionRepository
{
     private readonly IDbConnectionFactory _connectionFactory;
    public FuncionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Funcion> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Funcion>("SELECT * FROM Funcion").ToList();
    }

    public Funcion? GetById(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Funcion>(
            "SELECT * FROM Funcion WHERE IdFuncion = @IdFuncion",
            new { IdFuncion = id });
    }

    public int Add(Funcion funcion)
    {
        using var _connection = _connectionFactory.CreateConnection();        
        string sql = @"
                INSERT INTO Funcion (FechaHora, IdEvento, IdLocal, Estado)
                VALUES (@FechaHora, @IdEvento, @IdLocal, @Estado);
                SELECT LAST_INSERT_ID();";

        int newId = _connection.ExecuteScalar<int>(sql, funcion);
        funcion.IdFuncion = newId;
        return newId;
    }

    public int Update(Funcion funcion)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                UPDATE Funcion 
                SET FechaHora = @FechaHora,
                    IdLocal = @IdLocal,
                    IdEvento = @IdEvento,
                    Estado = @Estado
                WHERE IdFuncion = @IdFuncion";

        int rows = _connection.Execute(sql, funcion );
        return rows > 0 ? funcion.IdFuncion : 0;
    }

    public int Delete(int id)
    {      
        using var _connection = _connectionFactory.CreateConnection();  
        string deleteSql = "DELETE FROM Funcion WHERE IdFuncion = @IdFuncion";
        int rows = _connection.Execute(deleteSql, new { IdFuncion = id });
        return rows > 0 ? id : 0;
    }
}