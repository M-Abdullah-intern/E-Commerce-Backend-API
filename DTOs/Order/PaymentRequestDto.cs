namespace ECommerceAPI.DTOs.OrderDTOs
{
    public class PaymentRequestDto
    {
        public int OrderId { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;
    }
}