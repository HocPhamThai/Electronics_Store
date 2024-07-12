using System.ComponentModel.DataAnnotations;
namespace Electronics_Store.Shared.Responder;

public class SummaryOrderResponder
{
    [Key]
    public int id { get; set; }
    public decimal totalPrice { get; set; }
    public DateTime orderDate { get; set; }
    
    public string product { get; set; }
    public string productImageUrl { get; set; }
}