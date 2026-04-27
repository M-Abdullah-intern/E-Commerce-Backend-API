namespace ECommerceAPI.DTOs.ProductDTOs
{
    public class ProductQueryParams
    {
        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        // Filtering
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Sorting
        public string? SortBy { get; set; }  // price or name
        public string? SortOrder { get; set; } = "asc"; // asc / desc
    }
}