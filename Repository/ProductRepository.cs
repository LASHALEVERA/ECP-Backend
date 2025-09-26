using ECPAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ECPAPI.Repository
{
    public class ProductRepository : CategoryRepository<Product>, IProductRepository
    {
        private readonly ECPDbContext _dbContext;

        public ProductRepository(ECPDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Product>> GetProductsByNewListed(int NewListed)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Product>> SearchProductsAsync(int pageIndex, int pageSize, string? keywords, decimal? priceMin, decimal? priceMax)
        {
            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(p => p.Name.Contains(keywords) || p.Description.Contains(keywords));
            }

            if (priceMin.HasValue)
            {
                query = query.Where(p => p.Price >= priceMin.Value);
            }

            if (priceMax.HasValue)
            {
                query = query.Where(p => p.Price <= priceMax.Value);
            }

            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


    }
}
