using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace sve.Repositories
{
    public class SectorRepository : ISectorRepository
    {
        private readonly IConfiguration _configuration;

        public SectorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public List<Sector> GetAll()
        {
            using var db = Connection;
            return db.Query<Sector>("SELECT * FROM Sector").ToList();
        }

        public Sector? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Sector>(
                "SELECT * FROM Sector WHERE IdSector = @IdSector",
                new { IdSector = id });
        }

        public int Add(Sector sector)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Sector (Nombre, Capacidad, IdLocal)
                VALUES (@Nombre, @Capacidad, @IdLocal);
                SELECT LAST_INSERT_ID();";
            int newId = db.ExecuteScalar<int>(sql, sector);
            sector.IdSector = newId;
            return newId;
        }

        public bool Update(int id, Sector sector)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Sector 
                SET Nombre = @Nombre, Capacidad = @Capacidad, IdLocal = @IdLocal
                WHERE IdSector = @IdSector";
            int rows = db.Execute(sql, new { sector.Nombre, sector.Capacidad, sector.IdLocal, IdSector = id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Sector WHERE IdSector = @IdSector";
            int rows = db.Execute(sql, new { IdSector = id });
            return rows > 0;
        }
    }
}
