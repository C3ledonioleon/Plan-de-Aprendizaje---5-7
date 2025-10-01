using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace sve.Repositories
{
    public class LocalRepository : ILocalRepository
    {
        private readonly IConfiguration _configuration;

        public LocalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public List<Local> GetAll()
        {
            using var db = Connection;
            return db.Query<Local>("SELECT * FROM Local").ToList();
        }

        public Local? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Local>(
                "SELECT * FROM Local WHERE IdLocal = @IdLocal",
                new { IdLocal = id });
        }

        public int Add(Local local)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Local (Nombre, Direccion, CapacidadTotal)
                VALUES (@Nombre, @Direccion, @CapacidadTotal);
                SELECT LAST_INSERT_ID();";
            int newId = db.ExecuteScalar<int>(sql, local);
            local.IdLocal = newId;
            return newId;
        }

        public bool Update(int id, Local local)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Local 
                SET Nombre = @Nombre,
                    Direccion = @Direccion, 
                    CapacidadTotal = @CapacidadTotal
                WHERE IdLocal = @IdLocal";
            int rows = db.Execute(sql, new { local.Nombre, local.Direccion, local.CapacidadTotal, IdLocal = id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Local WHERE IdLocal = @IdLocal";
            int rows = db.Execute(sql, new { IdLocal = id });
            return rows > 0;
        }
    }
}
