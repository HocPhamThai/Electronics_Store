using System.ComponentModel.DataAnnotations.Schema;
namespace Electronics_Store.Shared.Order;

public class OrderProduct
{
    public Product product { get; set; }
    public int productId { get; set; }
    
    public ProductVariety productVariety { get; set; }
    public int productVarietyId { get; set; }
    
    public Order order { get; set; }
    public int orderId { get; set; }
    
    public int quantity { get; set; }
    [Column(TypeName = "decimal(20,2)")]
    public decimal orderProductPrice { get; set; }
}