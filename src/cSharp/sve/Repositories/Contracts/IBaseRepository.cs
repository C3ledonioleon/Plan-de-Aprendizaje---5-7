namespace sve.Repositories.Contracts;

public interface IBaseRepository<T>
{
    List<T> GetAll();
    T GetById(int id);
    int Add(T entity);
    int Update(int id,T entity);
    int Delete(int id);
}
