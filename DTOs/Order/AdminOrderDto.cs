namespace ECommerceAPI.DTOs.OrderDTOs
{
    public class AdminOrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
