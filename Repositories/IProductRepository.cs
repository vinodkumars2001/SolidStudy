using SolidStudy.DTOs;
using SolidStudy.Models;
using System.Collections;

namespace SolidStudy.Repositories
{
    public interface IProductRepository
    {
        public Task<Product> CreateProductAsync(Product request);
        public Task<Product?> GetProductByIdAsync(Guid id);
        public Task<IEnumerable<Product?>> GetAllProductAsync();

    }
}
