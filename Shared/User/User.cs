namespace Electronics_Store.Shared.User;

public class User
{
    public int userId { get; set; }
    
    public string userEmail { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    
    public byte[] userPasswordHashed { get; set; }
    public byte[] userPasswordSalt { get; set; }
    
    public Address? address {get; set;}
    
    public DateTime accountCreatedDate { get; set; } = DateTime.Now;
}