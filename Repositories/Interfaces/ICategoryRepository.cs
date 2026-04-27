using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();

        Task<bool> Exists(string name);

        Task Add(Category category);

        Task SaveChangesAsync();
    }
}