namespace ECommerceAPI.DTOs.OrderDTOs
{
    public class AdminOrderDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string ShippingFullName { get; set; }
        public string ShippingPhoneNumber { get; set; }
        public string ShippingStreetAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingCountry { get; set; }
    }
}
