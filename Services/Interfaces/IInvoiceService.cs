using ECommerceAPI.DTOs.Invoice;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> GenerateInvoiceAsync(int orderId);
    }
}