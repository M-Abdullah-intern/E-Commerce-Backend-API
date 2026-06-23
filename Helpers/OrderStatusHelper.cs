using ECommerceAPI.Enums;

namespace ECommerceAPI.Helpers
{
    public static class OrderStatusHelper
    {
        public static bool IsValidTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                (OrderStatus.Pending, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Cancelled) => true,
                _ => false
            };
        }
    }
}