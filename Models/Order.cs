using ECommerceAPI.Enums;

public class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public string ShippingFullName { get; set; }
    public string ShippingPhoneNumber { get; set; }
    public string ShippingStreetAddress { get; set; }
    public string ShippingCity { get; set; }
    public string ShippingState { get; set; }
    public string ShippingZipCode { get; set; }
    public string ShippingCountry { get; set; }
}