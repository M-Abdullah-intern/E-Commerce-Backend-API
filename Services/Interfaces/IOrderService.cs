using ECommerceAPI.DTOs.OrderDTOs;
using ECommerceAPI.Enums;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services.Interfaces
{
        public interface IOrderService
    {
        Task PlaceOrder(int userId, int shippingAddressId);
        Task<List<OrderDto>> GetUserOrders(int userId);
       
        //Admin features
        Task<List<AdminOrderDto>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}