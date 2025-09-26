using ECPAPI.Data;

namespace ECPAPI.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<Product?> GetProductByIdAsync(int productId);
        Task SaveAsync();
        //Task CreateAsync(Order order);
    }
}
