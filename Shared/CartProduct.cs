namespace Electronics_Store.Shared;

public class CartProduct
{
    public int quantity { get; set; } = 1;
    
    public int productId { get; set; }
    public int productVarietyId { get; set; }
    public int userId { get; set; }
    
}