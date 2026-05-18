using ECommerceAPI.DTOs.ProductImage;

public class ProductUpdateDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public string? Brand { get; set; }
    public double Rating { get; set; }
    public bool IsFeatured { get; set; }
    public List<ProductImageCreateDto> ProductImageCreateDtos { get; set; }
            = new();
}