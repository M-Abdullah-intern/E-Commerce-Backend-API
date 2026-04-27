using ECommerceAPI.Data;
using ECommerceAPI.DTOs.OrderDTOs;
using ECommerceAPI.Enums;
using ECommerceAPI.Exceptions;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly AppDbContext _context;

        public PaymentService( AppDbContext context, IPaymentRepository paymentRepository)
        {
            _context = context;
            _paymentRepository = paymentRepository;
        }

        public async Task<string> ProcessPayment(int userId, PaymentRequestDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _paymentRepository.GetOrderWithItems(dto.OrderId, userId);

                if (order == null)
                    throw new NotFoundException("Order not found");

                if (order.Status == OrderStatus.Cancelled)
                    throw new BadRequestException("Cannot pay for cancelled order");

                if (order.PaymentStatus != PaymentStatus.Pending)
                    throw new BadRequestException("Payment already processed");

                // Convert string to enum
                if (!Enum.TryParse<PaymentStatus>(dto.PaymentStatus, true, out var newStatus))
                    throw new BadRequestException("Invalid payment status");

                // Failed Payment
                if (newStatus == PaymentStatus.Failed)
                {
                    order.PaymentStatus = PaymentStatus.Failed;

                    await _paymentRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return "Payment Failed";
                }

                // Paid Payment
                if (newStatus == PaymentStatus.Paid)
                {
                    order.PaymentStatus = PaymentStatus.Paid;
                    order.Status = OrderStatus.Processing;

                    var cart = await _paymentRepository.GetUserCart(userId);

                    if (cart != null && cart.CartItems.Any())
                    {
                        await _paymentRepository.RemoveCartItems(cart.CartItems);
                    }

                    await _paymentRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return "Payment Successful";
                }

                throw new BadRequestException("Unsupported payment status");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}