using System.Text.Json.Serialization;

public class AddToCartDto
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}