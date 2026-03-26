using SolidStudy.Models;

namespace SolidStudy.Repositories
{
    public class InMemoryRepository : IProductRepository
    {

        private readonly List<Product> _products = new List<Product>();
        public async Task<Product> CreateProductAsync(Product product)
        {
            _products.Add(product);
            return await Task.FromResult(product);
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult<Product?>(product);
        }
        public async Task<IEnumerable<Product?>> GetAllProductAsync()
        {
            return await Task.FromResult<IEnumerable<Product>>(_products.AsEnumerable());
        }
    }
}
