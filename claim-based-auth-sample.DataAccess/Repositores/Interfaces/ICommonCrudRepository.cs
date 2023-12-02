namespace claim_based_auth_sample.DataAccess;

public interface ICommonCrudRepository<T>
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T?> Get(int id);
    Task<IEnumerable<T>> List();
    Task Delete(T entity);
}
