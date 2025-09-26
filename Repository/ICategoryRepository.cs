using System.Linq.Expressions;

namespace ECPAPI.Repository
{
    public interface ICategoryRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> CreateAsync(T dbRecord);
        Task<T> UpdateAsync(T dbRecord);
        Task<bool> DeleteAsync(T dbRecord);
        Task<List<T>> GetAllFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);

    }
}
