using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace sve.Repositories
{
    public class FuncionRepository : IFuncionRepository
    {
        private readonly IConfiguration _configuration;

        public FuncionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public List<Funcion> GetAll()
        {
            using var db = Connection;
            return db.Query<Funcion>("SELECT * FROM Funcion").ToList();
        }

        public Funcion? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Funcion>(
                "SELECT * FROM Funcion WHERE IdFuncion = @IdFuncion",
                new { IdFuncion = id });
        }

        public int Add(Funcion funcion)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Funcion (FechaHora, IdEvento, IdLocal, Estado)
                VALUES (@FechaHora, @IdEvento, @IdLocal, @Estado);
                SELECT LAST_INSERT_ID();";

            int newId = db.ExecuteScalar<int>(sql, funcion);
            funcion.IdFuncion = newId;
            return newId;
        }

        public bool Update(int id, Funcion funcion)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Funcion 
                SET FechaHora = @FechaHora,
                    IdLocal = @IdLocal,
                    IdEvento = @IdEvento,
                    Estado = @Estado
                WHERE IdFuncion = @IdFuncion";

            int rows = db.Execute(sql, new { funcion.FechaHora, funcion.IdLocal, funcion.IdEvento, funcion.Estado, IdFuncion = id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var db = Connection;

            // Verificar si la función tiene entradas vendidas
            string checkSql = "SELECT COUNT(*) FROM Entrada WHERE IdFuncion = @IdFuncion";
            int count = db.ExecuteScalar<int>(checkSql, new { IdFuncion = id });

            if (count > 0)
                return false;

            string deleteSql = "DELETE FROM Funcion WHERE IdFuncion = @IdFuncion";
            int rows = db.Execute(deleteSql, new { IdFuncion = id });
            return rows > 0;
        }
    }
}
