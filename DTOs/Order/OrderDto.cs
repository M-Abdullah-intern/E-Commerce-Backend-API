namespace ECommerceAPI.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
