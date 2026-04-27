using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserId(int userId);

        Task<Cart?> GetCartWithItems(int userId);

        Task<CartItem?> GetCartItemById(int cartItemId);

        Task AddCart(Cart cart);

        Task SaveChangesAsync();

        Task RemoveCartItem(CartItem item);
    }
}