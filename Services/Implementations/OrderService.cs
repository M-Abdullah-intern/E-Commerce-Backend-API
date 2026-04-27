using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs.OrderDTOs;
using ECommerceAPI.Enums;
using ECommerceAPI.Exceptions;
using ECommerceAPI.Helpers;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(
    AppDbContext context,
    IOrderRepository orderRepository,
    IMapper mapper)
        {
            _context = context;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task PlaceOrder(int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _orderRepository.GetUserCartWithItems(userId);

                if (cart == null || !cart.CartItems.Any())
                    throw new BadRequestException("Cart is empty");

                decimal total = 0;

                var order = new Order
                {
                    UserId = userId,
                    Status = OrderStatus.Pending,
                    OrderItems = new List<OrderItem>()
                };

                foreach (var item in cart.CartItems)
                {
                    // Validate product exists
                    if (item.Product == null)
                        throw new NotFoundException($"Product not found (ID: {item.ProductId})");

                    // Validate quantity
                    if (item.Quantity <= 0)
                        throw new BadRequestException($"Invalid quantity for {item.Product.Name}");

                    // Check stock
                    if (item.Product.Stock < item.Quantity)
                        throw new BadRequestException($"Not enough stock for {item.Product.Name}");

                    // Always use DB price
                    total += item.Product.Price * item.Quantity;

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });

                    // Reduce stock
                    item.Product.Stock -= item.Quantity;
                }

                order.TotalAmount = total;

                await _orderRepository.AddOrder(order);
                await _orderRepository.RemoveCartItems(cart.CartItems);
                await _orderRepository.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderDto>> GetUserOrders(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        // Admin Features
        public async Task<List<AdminOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrders();
            return _mapper.Map<List<AdminOrderDto>>(orders);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
                return false;

            if (!OrderStatusHelper.IsValidTransition(order.Status, newStatus))
                throw new Exception("Invalid status transition");

            order.Status = newStatus;

            await _orderRepository.SaveChangesAsync();

            return true;
        }

    }
}
