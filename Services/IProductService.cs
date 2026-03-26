using SolidStudy.DTOs;

namespace SolidStudy.Services
{
    public interface IProductService
    {
            public Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
            public Task<ProductResponse?> GetProductByIdAsync(Guid id);
            public Task<IEnumerable<ProductResponse?>> GetAllProductAsync();
    }
}
