using sve.Models;
using sve.Repositories.Contracts;
using System.Data;
using Dapper;

namespace sve.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly IDbConnection _connection;

    public ClienteRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Cliente> GetAll()
    {
        return _connection.Query<Cliente>("SELECT * FROM Cliente").ToList();
    }

    public Cliente? GetById(int id)
    {
        return _connection.QueryFirstOrDefault<Cliente>(
            "SELECT * FROM Cliente WHERE IdCliente = @IdCliente", 
            new { IdCliente = id });
    }

    public int Add(Cliente cliente)
    {
        string sql = @"
        INSERT INTO Cliente (Nombre, DNI, Telefono, IdUsuario)
        VALUES (@Nombre, @DNI, @Telefono, @IdUsuario);
        SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, cliente);
        cliente.IdCliente = newId;
        return newId;
    }

    public int Update(int id, Cliente cliente)
    {
        string sql = @"
        UPDATE Cliente 
        SET Nombre = @Nombre,
            DNI = @DNI,
            Telefono = @Telefono,
            IdUsuario = @IdUsuario
        WHERE IdCliente = @IdCliente";
        int rows = _connection.Execute(sql, cliente );
        return rows > 0 ? cliente.IdCliente : 0;
    }

    public int Delete(int id)
    {
        string sql = "DELETE FROM Cliente WHERE IdCliente = @IdCliente";
        int rows = _connection.Execute(sql, new { IdCliente = id });
        return rows > 0 ? id : 0;
    }

}
