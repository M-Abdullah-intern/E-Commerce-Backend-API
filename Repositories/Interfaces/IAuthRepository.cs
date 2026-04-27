using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}