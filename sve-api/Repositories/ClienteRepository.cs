using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConfiguration _configuration;

        public ClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public List<Cliente> GetAll()
        {
            using var db = Connection;
            return db.Query<Cliente>("SELECT * FROM Cliente").ToList();
        }

        public Cliente? GetById(int id)
        {
            using var db = Connection;
            return db.QueryFirstOrDefault<Cliente>(
                "SELECT * FROM Cliente WHERE IdCliente = @IdCliente", 
                new { IdCliente = id });
        }

        public int Add(Cliente cliente)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO Cliente (Nombre, DNI, Telefono)
                VALUES (@Nombre, @DNI, @Telefono);
                SELECT LAST_INSERT_ID();";
            int newId = db.ExecuteScalar<int>(sql, cliente);
            cliente.IdCliente = newId;
            return newId;
        }

        public bool Update(int id, Cliente cliente)
        {
            using var db = Connection;
            string sql = @"
                UPDATE Cliente 
                SET Nombre = @Nombre,
                DNI = @DNI,
                Telefono = @Telefono
                WHERE IdCliente = @IdCliente";
            int rows = db.Execute(sql, new { cliente.Nombre, cliente.DNI, cliente.Telefono, IdCliente = id });
            return rows > 0;
        }

        public bool Delete(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Cliente WHERE IdCliente = @IdCliente";
            int rows = db.Execute(sql, new { IdCliente = id });
            return rows > 0;
        }
    }
}
