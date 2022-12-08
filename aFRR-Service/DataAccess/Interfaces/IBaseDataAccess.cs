namespace DataAccess.Interfaces;
public interface IBaseDataAccess<T>
{
    Task<int> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(params dynamic[] id);
    Task<bool> DeleteAsync(params dynamic[] id);
    Task<bool> UpdateAsync(T entity);
}