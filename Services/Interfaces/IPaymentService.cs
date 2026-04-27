using ECommerceAPI.DTOs.OrderDTOs;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> ProcessPayment(int userId, PaymentRequestDto dto);
    }
}