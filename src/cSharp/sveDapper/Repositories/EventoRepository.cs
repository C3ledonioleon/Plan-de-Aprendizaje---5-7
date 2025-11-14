using sveCore.Models;
using sveCore.Servicio.IRepositories;
using System.Data;
using Dapper;
using sveDapper.Factories;

namespace sveDapper.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EventoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Evento> GetAll()
        {
            using var _connection = _connectionFactory.CreateConnection();
            return _connection.Query<Evento>("SELECT * FROM Evento").ToList();
        }

        public Evento? GetById(int id)
        {
            using var _connection = _connectionFactory.CreateConnection();
            return _connection.QueryFirstOrDefault<Evento>(
                "SELECT * FROM Evento WHERE IdEvento = @IdEvento",
                new { IdEvento = id });
        }
        public int Add(Evento evento)
        {
            using var _connection = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Evento (Nombre, Descripcion , FechaInicio, FechaFin, Estado)
                VALUES (@Nombre, @Descripcion ,@FechaInicio, @FechaFin ,@Estado);
                SELECT LAST_INSERT_ID();";

            int newId =_connection.ExecuteScalar<int>(sql, evento);
            evento.IdEvento = newId;
            return newId;
        }
        public int Update(Evento evento)
        {
            using var _connection = _connectionFactory.CreateConnection();
            string sql = @"
                UPDATE Evento 
                SET Nombre = @Nombre,
                Descripcion = @Descripcion,
                FechaInicio = @FechaInicio,
                FechaFin = @FechaFin,
                Estado = @Estado
                WHERE IdEvento = @IdEvento";
            int rows = _connection.Execute(sql, evento );
            return rows > 0 ? evento.IdEvento  : 0;
        }
        
        public int Delete(int id)
        {
            using var _connection = _connectionFactory.CreateConnection();
            string sql = "DELETE FROM Evento WHERE IdEvento = @IdEvento";
            int rows = _connection.Execute(sql, new { IdEvento = id });
            return rows > 0 ? id : 0;
        }

    }
}