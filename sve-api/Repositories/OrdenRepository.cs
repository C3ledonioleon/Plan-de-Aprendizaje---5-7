using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace sve.Repositories
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly IConfiguration _configuration;

        public OrdenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public List<Orden> GetAll()
        {
            using var db = Connection;
            return db.Query<Orden>("SELECT * FROM Orden").ToList();
        }

        public Orden? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Orden>(
                "SELECT * FROM Orden WHERE IdOrden = @IdOrden",
                new { IdOrden = id });
        }

        public int Add(Orden orden)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Orden (Total, Fecha, IdCliente, IdTarifa, Estado)
                VALUES (@Total, @Fecha, @IdCliente, @IdTarifa, @Estado);
                SELECT LAST_INSERT_ID();";
            int newId = db.ExecuteScalar<int>(sql, orden);
            orden.IdOrden = newId;
            return newId;
        }

        public bool Update(int id, Orden orden)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Orden 
                SET Total = @Total, 
                    Fecha = @Fecha,
                    Estado = @Estado,
                    IdCliente = @IdCliente,
                    IdTarifa = @IdTarifa
                WHERE IdOrden = @IdOrden";
            int rows = db.Execute(sql, new { orden.Total, orden.Fecha, orden.Estado, orden.IdCliente, orden.IdTarifa, IdOrden = id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Orden WHERE IdOrden = @IdOrden";
            int rows = db.Execute(sql, new { IdOrden = id });
            return rows > 0;
        }
    }
}
