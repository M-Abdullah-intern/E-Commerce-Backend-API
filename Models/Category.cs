namespace ECommerceAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Navigation Propert
        public List<Product>? Products { get; set; }
    }
}
