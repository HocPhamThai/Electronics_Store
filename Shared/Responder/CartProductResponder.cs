namespace Electronics_Store.Shared.Responder;

public class CartProductResponder
{
    public int productId { get; set; }
    public int productVarietyId { get; set; }
    public int quantity { get; set; }
    public decimal price { get; set; }
    
    public string name { get; set; } = string.Empty;
    public string productVariety { get; set; } = string.Empty;
    public string imageUrl { get; set; } = string.Empty;
    
}