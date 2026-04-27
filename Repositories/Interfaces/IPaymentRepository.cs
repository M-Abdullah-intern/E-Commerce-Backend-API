using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Order?> GetOrderWithItems(int orderId, int userId);

        Task<Cart?> GetUserCart(int userId);

        Task RemoveCartItems(List<CartItem> items);

        Task SaveChangesAsync();
    }
}