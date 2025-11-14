using Dapper;
using MySql.Data.MySqlClient;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveDapper.Factories;
using System.Data;

namespace sveDapper.Repositories;

public class OrdenRepository : IOrdenRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly EntradaRepository entradaRepository;

    public OrdenRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        entradaRepository = new EntradaRepository(_connectionFactory);
    }

    public List<Orden> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Orden>("SELECT * FROM Orden").ToList();
    }

    public Orden? GetById(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Orden>(
            "SELECT * FROM Orden WHERE IdOrden = @IdOrden",
            new { IdOrden = id });
    }

    public int Add(Orden orden)
    {
        using var _connection = _connectionFactory.CreateConnection();
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
        using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                UPDATE Orden 
                SET Total = @Total, 
                    Fecha = @Fecha,
                    Estado = @Estado,
                    IdCliente = @IdCliente,
                    IdTarifa = @IdTarifa
                WHERE IdOrden = @IdOrden";
        int rows = _connection.Execute(sql, orden);

        Entrada entrada = new Entrada();

        entrada.Precio = orden.Total;
        entrada.IdCliente = orden.IdCliente;
        entrada.IdOrden = orden.IdOrden;
        entrada.IdTarifa = orden.IdTarifa;
        entrada.IdFuncion = _connection.QueryFirstOrDefault<Funcion>("SELECT * FROM Funcion JOIN Tarifa USING (IdFuncion) WHERE IdTarifa = @ID", new { ID = orden.IdTarifa })!.IdFuncion;
        entrada.Estado = EstadoEntrada.Activa;
        entradaRepository.Add(entrada);
        return rows > 0 ? orden.IdOrden : 0;
    }

    public int Delete(int id)
    {
        using var _connection = _connectionFactory.CreateConnection();
        string sql = "DELETE FROM Orden WHERE IdOrden = @IdOrden";
        int rows = _connection.Execute(sql, new { IdOrden = id });
        return rows > 0 ? id : 0;
    }

    // ✅ IMPLEMENTACIÓN EXACTA QUE PEDISTE
    public bool PagarOrden(int idOrden)
    {
        using var _connection = _connectionFactory.CreateConnection();
        var result = _connection.QueryFirstOrDefault<dynamic>(
            "CALL PagarOrden(@IdOrden);",
            new { IdOrden = idOrden }
        );

        return result != null && result.Mensaje == "Orden pagada y entrada creada.";
    }

// ✅ Método para cancelar la orden
    public bool CancelarOrden(int idOrden)
    {
        using var _connection = _connectionFactory.CreateConnection();
        var result = _connection.QueryFirstOrDefault<dynamic>(
            "CALL CancelarOrden(@IdOrden);",
            new { IdOrden = idOrden }
        );

        // Comprobamos que el mensaje sea el esperado del procedimiento
        return result != null && result.Mensaje == "Orden cancelada y stock incrementado +1.";
    }

}