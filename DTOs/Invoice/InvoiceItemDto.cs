namespace ECommerceAPI.DTOs.Invoice
{
    public class InvoiceItemDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}