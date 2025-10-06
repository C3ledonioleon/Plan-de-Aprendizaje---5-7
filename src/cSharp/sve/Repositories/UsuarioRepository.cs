using Dapper;
using sve.Models;
using sve.Repositories.Contracts;
using System.Data;

namespace sve.Services;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnection _connection;

    public UsuarioRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Usuario> GetAll()
    {
        return _connection.Query<Usuario>("SELECT * FROM Usuario").ToList();
    }

    public Usuario? GetById(int id)
    {
        return _connection.QueryFirstOrDefault<Usuario>(
            "SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario",
            new { IdUsuario = id });
    }

    public Usuario? GetByEmail(string email)
    {
        return _connection.QueryFirstOrDefault<Usuario>(
            "SELECT * FROM Usuario WHERE Email = @Email",
            new { Email = email });
    }

    public int Add(Usuario usuario)
    {
        string sql = @"
        INSERT INTO Usuario (Username, Email, Password)
        VALUES (@Username, @Email, @Password);
        SELECT LAST_INSERT_ID();";
        int newId = _connection.ExecuteScalar<int>(sql, usuario);
        usuario.IdUsuario = newId;
        return newId;
    }

    public int Update(Usuario usuario)
    {
        string sql = @"
        UPDATE Usuario 
        SET Username = @Username,
            Email = @Email,
            Password = @Password,
            Rol = @Rol,
        WHERE IdUsuario = @IdUsuario";
        int rows = _connection.Execute(sql, usuario);
        return rows > 0 ? usuario.IdUsuario : 0;
    }

    public int Delete(int id)
    {
        var checkSql = "SELECT COUNT(*) FROM Cliente WHERE IdUsuario = @IdUsuario";
        var count = _connection.ExecuteScalar<int>(checkSql, new { IdUsuario = id });

        if (count > 0)
            return 0;

        string sql = "DELETE FROM Usuario WHERE IdUsuario = @IdUsuario";
        int rows = _connection.Execute(sql, new { IdUsuario = id });
        return rows > 0 ? id : 0;
    }

    public Usuario? GetByRefreshToken(string refreshToken)
    {
        string sql = @"
                SELECT * FROM Usuario 
                WHERE RefreshToken = @RefreshToken 
                AND RefreshTokenExpiracion > NOW()";
        return _connection.QueryFirstOrDefault<Usuario>(sql, new { RefreshToken = refreshToken });
    }

    public void UpdateRefreshToken(string email, string refreshToken, DateTime expiracion)
    { 
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
