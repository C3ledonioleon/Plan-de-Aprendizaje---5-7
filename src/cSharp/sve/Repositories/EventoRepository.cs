using sve.Models;
using sve.Repositories.Contracts;
using System.Data;
using Dapper;

namespace sve.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly IDbConnection _connection;

        public EventoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public List<Evento> GetAll()
        {
  
            return _connection.Query<Evento>("SELECT * FROM Evento").ToList();
        }

        public Evento? GetById(int id)
        {
    
            return _connection.QueryFirstOrDefault<Evento>(
                "SELECT * FROM Evento WHERE IdEvento = @IdEvento",
                new { IdEvento = id });
        }
        public int Add(Evento evento)
        {

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
            string sql = @"
                UPDATE Evento 
                SET Nombre = @Nombre,
                Descripcion = @Descripcion
                FechaInicio = @FechaInicio,
                FechaFin = @FechaFin,
                Estado = @Estado
                WHERE IdEvento = @IdEvento";
            int rows = _connection.Execute(sql, evento );
            return rows > 0 ? evento.IdEvento  : 0;
        }
        
        public int Delete(int id)
        {
            string sql = "DELETE FROM Evento WHERE IdEvento = @IdEvento";
            int rows = _connection.Execute(sql, new { IdEvento = id });
            return rows > 0 ? id : 0;
        }

    }
}