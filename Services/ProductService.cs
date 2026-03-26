using SolidStudy.DTOs;
using SolidStudy.Models;
using SolidStudy.Repositories;

namespace SolidStudy.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }
            if (request.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                CreatedDate = DateTime.UtcNow
            };
            var createdProduct = await _repository.CreateProductAsync(product);

            return MapToResponse(createdProduct);
        }
        public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return MapToResponse(product);
            
        }
        public async Task<IEnumerable<ProductResponse?>> GetAllProductAsync()
        {
            var products = await _repository.GetAllProductAsync();
            return  products.Select(p => p == null ? null : MapToResponse(p));
        }
        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedDate = product.CreatedDate
            };
        }
    }
}
