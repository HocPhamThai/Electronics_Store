namespace Electronics_Store.Shared.Responder;

public class DetailOrderResponder
{
    public decimal totalPrice { get; set; }
    public DateTime orderDate { get; set; }
    
    public List<DetailOrderProductResponder> products { get; set; }
}