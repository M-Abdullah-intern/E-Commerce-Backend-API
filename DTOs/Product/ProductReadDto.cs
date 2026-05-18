using ECommerceAPI.DTOs.ProductImage;

public class ProductReadDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Brand { get; set; }
    public double Rating { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    public List<ProductImageDto> ProductImages { get; set; }

}