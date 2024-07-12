namespace Electronics_Store.Shared.Responder;

public class DetailOrderProductResponder
{
    public int productId { get; set; }
    public int quantity { get; set; }
    public decimal totalPrice { get; set; }
    
    public string productName { get; set; }
    public string productVariety { get; set; }
    public string productImageUrl { get; set; }
}