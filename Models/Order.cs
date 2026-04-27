using ECommerceAPI.Enums;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

}