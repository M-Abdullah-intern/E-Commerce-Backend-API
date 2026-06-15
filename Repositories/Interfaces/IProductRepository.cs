using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        Task<Product> GetByIdAsync(int id);

        Task<PaginatedResult<Product>> GetFilteredProductsAsync(ProductQueryParams queryParams);

        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}