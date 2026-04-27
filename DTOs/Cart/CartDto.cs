namespace ECommerceAPI.DTOs.CartDTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public List<CartItemDto> Items { get; set; }
    }
}