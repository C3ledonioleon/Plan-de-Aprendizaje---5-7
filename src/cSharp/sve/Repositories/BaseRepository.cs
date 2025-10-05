using Dapper;
using sve.Models;
using sve.Repositories.Contracts;
using System.Data;

namespace sve.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly IDbConnection _connection;

    public BaseRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public T GetById(int id)
    {
        var sql = $"SELECT * FROM {typeof(T).Name} WHERE Id{typeof(T).Name} = @Id";
        var sql2 = $"SELECT * FROM Cliente WHERE IdCliente = @Id";
        return _connection.QuerySingleOrDefault<T>(sql, new { Id = id });
    }

    public IEnumerable<T> GetAll()
    {
        var sql = $"SELECT * FROM {typeof(T).Name}";
        return _connection.Query<T>(sql);
    }

    public int Add(T entity)
    {
        var props = typeof(T).GetProperties()
            .Where(p => p.Name != typeof(T).Name); // excluye la PK

        var columns = string.Join(",", props.Select(p => p.Name));
        var values = string.Join(",", props.Select(p => "@" + p.Name));

        var sql = $@"
            INSERT INTO {typeof(T).Name} ({columns})
            VALUES ({values});
            SELECT CAST(SCOPE_IDENTITY() as int);";

        return _connection.QuerySingle<int>(sql, entity);
    }

    public int Update(T entity)
    {
        var props = typeof(T).GetProperties()
            .Where(p => p.Name != typeof(T).Name);

        var setClause = string.Join(",", props.Select(p => $"{p.Name}=@{p.Name}"));

        var sql = $"UPDATE {typeof(T).Name} SET {setClause} WHERE {typeof(T).Name} = @{typeof(T).Name}";
        return _connection.Execute(sql, entity);
    }

    public int Delete(int id)
    {
        var sql = $"DELETE FROM {typeof(T).Name} WHERE {typeof(T).Name} = @Id";
        return _connection.Execute(sql, new { Id = id });
    }
}