using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseDataAccess;
public interface IBaseDataAccess<T>
{
    Task<int> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(params dynamic[] id);
    Task<bool> DeleteAsync(params dynamic[] id);
    Task<bool> UpdateAsync(T entity);
}