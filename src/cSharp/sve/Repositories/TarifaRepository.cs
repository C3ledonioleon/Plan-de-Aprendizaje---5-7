using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using System.Data;

namespace sve.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
        private readonly IConfiguration _connectionString;

        // Ahora recibe IConfiguration
        public TarifaRepository(IConfiguration configuration)
        {
            _connectionString = configuration ;
        }

        private IDbConnection GetConnection() => new MySqlConnection(_connectionString.GetConnectionString("DefaultConnection"));

        public List<Tarifa> GetAll()
        {
            using var db = GetConnection();
            return db.Query<Tarifa>("SELECT * FROM Tarifa").ToList();
        }

        public Tarifa? GetById(int id)
        {
            using var db = GetConnection();
            return db.QueryFirstOrDefault<Tarifa>(
                "SELECT * FROM Tarifa WHERE IdTarifa = @IdTarifa",
                new { IdTarifa = id });
        }

        public int Add(Tarifa tarifa)
        {
            using var db = GetConnection();
            string sql = @"
                INSERT INTO Tarifa (Precio, Stock, IdSector, IdFuncion)
                VALUES (@Precio, @Stock, @IdSector, @IdFuncion);
                SELECT LAST_INSERT_ID();";
            int newId = db.ExecuteScalar<int>(sql, tarifa);
            tarifa.IdTarifa = newId;
            return newId;
        }

        public bool Update(int id, Tarifa tarifa)
        {
            using var db = GetConnection();
            string sql = @"
                UPDATE Tarifa 
                SET Precio = @Precio,
                    Stock = @Stock,
                    IdSector = @IdSector,
                    IdFuncion = @IdFuncion
                WHERE IdTarifa = @IdTarifa";
            int rows = db.Execute(sql, new    {tarifa.Precio, tarifa.Stock, tarifa.IdSector, tarifa.IdFuncion, IdTarifa = id});
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var db = GetConnection();
            string sql = "DELETE FROM Tarifa WHERE IdTarifa = @IdTarifa";
            int rows = db.Execute(sql, new { IdTarifa = id });
            return rows > 0;
        }
    }
}
