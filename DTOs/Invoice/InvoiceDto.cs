namespace ECommerceAPI.DTOs.Invoice
{
    public class InvoiceDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public DateTime InvoiceDate { get; set; }

        public List<InvoiceItemDto> Items { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
    }
}