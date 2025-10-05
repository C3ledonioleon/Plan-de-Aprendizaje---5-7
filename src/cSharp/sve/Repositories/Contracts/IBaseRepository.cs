namespace sve.Repositories.Contracts;

public interface IBaseRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    int Add(T entity);
    int Update(T entity);
    int Delete(int id);
}
