using ECommerceAPI.DTOs.ProductDTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllProducts();
        Task<IEnumerable<ProductReadDto>> GetProductsByCategory(int categoryId);
        Task<IEnumerable<ProductReadDto>> SearchProducts(string keyword);

        Task<ProductReadDto> GetProductById(int id);

        Task<IEnumerable<ProductReadDto>> GetFilteredProducts(ProductQueryParams queryParams);

        Task<ProductCreatedDto> AddProduct(ProductCreateDto dto);
        Task UpdateProduct(int id, ProductUpdateDto dto);
        Task DeleteProduct(int id);
    }
}