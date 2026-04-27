using ECommerceAPI.DTOs.Category;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAll();

        Task CreateCategory(Category category);
    }
}