namespace ECommerceAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public double Rating { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
             = new List<ProductImage>();

    }

}