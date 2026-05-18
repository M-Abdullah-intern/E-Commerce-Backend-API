using ECommerceAPI.DTOs.ProductImage;

namespace ECommerceAPI.DTOs.ProductDTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock {  get; set; }
        public int CategoryId { get; set; }
        public string? Brand { get; set; }
        public double Rating { get; set; }
               
        public List<ProductImageCreateDto> ProductImageCreateDtos { get; set; }
            = new();
    }
}
