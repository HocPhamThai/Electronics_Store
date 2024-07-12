using System.ComponentModel.DataAnnotations.Schema;
namespace Electronics_Store.Shared.Order;

public class Order
{
    public int orderId { get; set; }
    [Column(TypeName = "decimal(20,2)")]
    public decimal orderPrice { get; set; }
    public DateTime orderDate { get; set; } = DateTime.Now;
    
    public List<OrderProduct> orderProducts { get; set; }
    
    public int userId { get; set; }
}