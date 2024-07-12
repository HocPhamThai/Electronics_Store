using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics_Store.Shared;

public class Product
{
    public int ProductId { get; set; }
    public int ProductCategoryId { get; set; }
    public bool IsTopProduct { get; set; }
    
    public bool isDeleted { get; set; } = false;
    public bool isViewable { get; set; } = true;
    
    [Required]
    public string ProductName { get; set; } = string.Empty;
    public string ProductDetail { get; set; } = string.Empty;
    public string ProductImageUrl { get; set; } = string.Empty; 
    
    public Category? ProductCategory { get; set; }
    
    public List<Picture> Pictures { get; set; } = new();
    public List<ProductVariant>? ProductVariants { get; set; }
    
    
    [NotMapped]
    public bool isNew { get; set; } = false;
    [NotMapped]
    public bool isModifying { get; set; } = false;
}