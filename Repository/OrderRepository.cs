using ECPAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ECPAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECPDbContext _context;

        public OrderRepository(ECPDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
            
        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        //public async Task AddAsync(Order order)
        //{
        //    await _context.Orders.AddAsync(order);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order; 
        }


        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        ///!!!
        //public async Task CreateAsync(Order orders)
        //{
        //    await _context.Orders.AddAsync(orders);
        //    await _context.SaveChangesAsync();
        //}
    }
}
