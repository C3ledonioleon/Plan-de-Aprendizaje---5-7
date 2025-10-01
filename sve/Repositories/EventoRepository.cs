using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;

namespace sve.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly IConfiguration _configuration;

        public EventoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));



        public List<Evento> GetAll()
        {
            using var db = Connection;
            return db.Query<Evento>("SELECT * FROM Evento").ToList();
        }

        public Evento? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Evento>(
                "SELECT * FROM Evento WHERE IdEvento = @IdEvento",
                new { IdEvento = id });
        }
        public int Add(Evento evento)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Evento (Nombre, Genero, FechaInicio, FechaFin,Estado)
                VALUES (@Nombre, @Genero, @FechaInicio, @FechaFin ,@Estado);
                SELECT LAST_INSERT_ID();";

            int newId = db.ExecuteScalar<int>(sql, evento);
            evento.IdEvento = newId;
            return newId;
        }
        public bool Update(int id, Evento evento)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Evento 
                SET Nombre = @Nombre,
                Descripcion = @Decripcion,
                FechaInicio = @FechaInicio,
                FechaFin = @FechaFin,
                Estado = @Estado
                WHERE IdEvento = @IdEvento";
            int rows = db.Execute(sql, new { evento.Nombre, evento.Descripcion, evento.FechaInicio, evento.FechaFin, evento.Estado , IdEvento = id });
            return rows > 0;
        }
        
        public bool Delete(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Evento WHERE IdEvento = @IdEvento";
            int rows = db.Execute(sql, new { IdEvento = id });
            return rows > 0;
        }

    }
}