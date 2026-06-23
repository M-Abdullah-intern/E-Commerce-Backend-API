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
        private readonly IAddressRepository _addressRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(
    AppDbContext context,
    IOrderRepository orderRepository,
    IAddressRepository addressRepository,
    IMapper mapper)
        {
            _context = context;
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task PlaceOrder(int userId, int shippingAddressId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _orderRepository.GetUserCartWithItems(userId);

                if (cart == null || !cart.CartItems.Any())
                    throw new BadRequestException("Cart is empty");

                var address = await _addressRepository.GetById(shippingAddressId);

                if (address == null || address.UserId != userId)
                    throw new NotFoundException("Shipping address not found");

                decimal total = 0;

                var order = new Order
                {
                    UserId = userId,
                    Status = OrderStatus.Pending,
                    OrderItems = new List<OrderItem>(),
                    ShippingFullName = address.FullName,
                    ShippingPhoneNumber = address.PhoneNumber,
                    ShippingStreetAddress = address.StreetAddress,
                    ShippingCity = address.City,
                    ShippingState = address.State,
                    ShippingZipCode = address.ZipCode,
                    ShippingCountry = address.Country
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

        public async Task CancelOrder(int userId, int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
                throw new NotFoundException("Order not found");

            if (order.UserId != userId)
                throw new UnauthorizedAccessException("You can only cancel your own orders");

            if (!OrderStatusHelper.IsValidTransition(order.Status, OrderStatus.Cancelled))
                throw new Exception("Order cannot be cancelled in its current state");

            order.Status = OrderStatus.Cancelled;

            await _orderRepository.SaveChangesAsync();
        }

        public async Task ConfirmDelivery(int userId, int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
                throw new NotFoundException("Order not found");

            if (order.UserId != userId)
                throw new UnauthorizedAccessException("You can only confirm delivery for your own orders");

            if (!OrderStatusHelper.IsValidTransition(order.Status, OrderStatus.Delivered))
                throw new Exception("Order cannot be marked as delivered in its current state");

            order.Status = OrderStatus.Delivered;

            await _orderRepository.SaveChangesAsync();
        }

    }
}
