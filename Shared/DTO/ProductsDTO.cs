namespace Electronics_Store.Shared.DTO;

public class ProductsDTO
{
    public int numOfPages { get; set; }
    public int currentPageIndex { get; set; }
    
    public List<Product> products { get; set; } = new();
}