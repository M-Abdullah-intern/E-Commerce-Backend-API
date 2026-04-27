using ECommerceAPI.DTOs.CartDTOs;

namespace ECommerceAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCart(int userId);
        Task AddToCart(int userId, int productId, int quantity);
        Task UpdateQuantity(int cartItemId, int quantity, int userId);
        Task RemoveItem(int cartItemId, int userId);
    }
}