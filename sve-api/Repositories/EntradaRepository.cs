using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Repositories
{
    public class EntradaRepository : IEntradaRepository
    {
        private readonly IConfiguration _configuration;

        public EntradaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public List<Entrada> GetAll()
        {
            using var db = Connection;
            return db.Query<Entrada>("SELECT * FROM Entrada").ToList();
        }

        public Entrada? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Entrada>(
                "SELECT * FROM Entrada WHERE IdEntrada = @IdEntrada",
                new { IdEntrada = id });
        }

        public int Add(Entrada entrada)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Entrada (Precio, IdCliente, IdFuncion)
                VALUES (@Precio, @IdCliente, @IdFuncion);
                SELECT LAST_INSERT_ID();";

            int newId = db.ExecuteScalar<int>(sql, entrada);
            entrada.IdEntrada = newId;
            return newId;
        }

        public bool Update(int id, Entrada entrada)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Entrada 
                SET Precio = @Precio,
                IdOrden = @IdOrden,
                IdTarifa = @IdTarifa
                Estado = @Estado
                WHERE IdEntrada = @IdEntrada";
            int rows = db.Execute(sql, new { entrada.Precio, entrada.IdOrden, entrada.IdTarifa, entrada.Estado , IdEntrada = id });
            return rows > 0;
        }
        public bool Delete(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Entrada WHERE IdEntrada = @IdEntrada";
            int rows = db.Execute(sql, new { IdEntrada = id });
            return rows > 0;
        }
    }
}
        