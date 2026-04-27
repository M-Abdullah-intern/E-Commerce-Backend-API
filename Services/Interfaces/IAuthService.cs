using ECommerceAPI.DTOs.AuthDTOs;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}