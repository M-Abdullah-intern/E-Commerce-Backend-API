using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Cart> GetUserCartWithItems(int userId);

        Task AddOrder(Order order);

        Task RemoveCartItems(List<CartItem> cartItems);

        Task SaveChangesAsync();

        Task<List<Order>> GetUserOrders(int userId);

        Task<List<Order>> GetAllOrders();

        Task<Order?> GetOrderById(int orderId);

        Task<Order?> GetOrderWithDetailsAsync(int orderId);
    }
}