using ECommerceAPI.DTOs.Invoice;
using ECommerceAPI.Helpers;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IOrderRepository _orderRepository;

        public InvoiceService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<InvoiceDto> GenerateInvoiceAsync(int orderId)
        {
            // 1. Get Order
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            // 2. Map Items
            var items = order.OrderItems.Select(oi => new InvoiceItemDto
            {
                ProductName = oi.Product.Name,
                Price = oi.Price,
                Quantity = oi.Quantity
            }).ToList();

            // 3. Calculate Subtotal
            var subtotal = items.Sum(i => i.Total);

            var tax = InvoiceHelper.CalculateTax(subtotal);
            var total = InvoiceHelper.CalculateTotal(subtotal);

            // 6. Return DTO
            return new InvoiceDto
            {
                OrderId = order.Id,
                CustomerName = order.User.Name,
                CustomerEmail = order.User.Email,
                InvoiceDate = DateTime.UtcNow,

                Items = items,
                SubTotal = subtotal,
                Tax = tax,
                TotalAmount = total
            };
        }
    }
}