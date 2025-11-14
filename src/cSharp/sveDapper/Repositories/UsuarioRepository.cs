using Dapper;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveDapper.Factories;
using System.Data;

namespace sveDapper.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Usuario> GetAll()
    {
        using var _connection = _connectionFactory.CreateConnection();
        return _connection.Query<Usuario>("SELECT * FROM Usuario").ToList();
    }

    public Usuario? GetById(int id)
    {
         using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Usuario>(
            "SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario",
            new { IdUsuario = id });
    }

    public Usuario? GetByEmail(string email)
    {
         using var _connection = _connectionFactory.CreateConnection();
        return _connection.QueryFirstOrDefault<Usuario>(
            "SELECT * FROM Usuario WHERE Email = @Email",
            new { Email = email });
    }

    public int Add(Usuario usuario)
    {
         using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
        INSERT INTO Usuario (Username, Email, Password,Rol)
        VALUES (@Username, @Email, @Password , @Rol);
        SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, usuario);
        usuario.IdUsuario = newId;
        return newId;
    }

    public int Update(Usuario usuario)
    {
         using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
        UPDATE Usuario 
        SET Username = @Username,
            Email = @Email,
            Password = @Password,
            Rol = @Rol
        WHERE IdUsuario = @IdUsuario";
        int rows = _connection.Execute(sql, usuario);
        return rows > 0 ? usuario.IdUsuario : 0;
    }

    public int Delete(int id)
    {
         using var _connection = _connectionFactory.CreateConnection();
        string sql = "DELETE FROM Usuario WHERE IdUsuario = @IdUsuario";
        int rows = _connection.Execute(sql, new { IdUsuario = id });
        return rows > 0 ? id : 0;
    }

    public Usuario? GetByRefreshToken(string refreshToken)
    {
         using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                SELECT * FROM Usuario 
                WHERE RefreshToken = @RefreshToken 
                AND RefreshTokenExpiracion > NOW()";
        return _connection.QueryFirstOrDefault<Usuario>(sql, new { RefreshToken = refreshToken });
    }

    public void UpdateRefreshToken(string email, string refreshToken, DateTime expiracion)
    { 
         using var _connection = _connectionFactory.CreateConnection();
        string sql = @"
                UPDATE Usuario 
                SET RefreshToken = @RefreshToken, 
                    RefreshTokenExpiracion = @Expiracion
                WHERE Email = @Email";

        _connection.Execute(sql, new
        {
            RefreshToken = refreshToken,
            Expiracion = expiracion,
            Email = email
        });
    }
}
