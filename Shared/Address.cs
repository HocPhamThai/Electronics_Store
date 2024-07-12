namespace Electronics_Store.Shared;

public class Address
{
    public int id { get; set; }
    public int userId { get; set; }
    
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string country { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public string district { get; set; } = string.Empty;
    public string street { get; set; } = string.Empty;
    
    public string zip { get; set; } = string.Empty;
}