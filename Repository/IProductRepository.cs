using ECPAPI.Data;

namespace ECPAPI.Repository
{
    public interface IProductRepository : ICategoryRepository<Product>
    {
        Task<List<Product>> GetProductsByNewListed(int NewListed);
        Task<List<Product>> SearchProductsAsync(int pageIndex, int pageSize, string? keywords, decimal? priceMin, decimal? priceMax);
    }
}
