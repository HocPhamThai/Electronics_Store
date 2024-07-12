using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Electronics_Store.Shared.Responder;

namespace Electronics_Store.Shared;

public class ProductVariant
{
    [JsonIgnore]
    public Product? product { get; set; }
    public int productId { get; set; }
    
    public bool isDeleted { get; set; } = false;
    public bool isViewable { get; set; } = true;
    
    public ProductVariety? productVariety { get; set; }
    public int productVarietyId { get; set; }
    
    [Column(TypeName = "Decimal(20,2)")]
    public decimal price { get; set; }
    [Column(TypeName = "Decimal(20,2)")]
    public decimal initialPrice { get; set; }
    
    
    [NotMapped]
    public bool isNew { get; set; } = false;
    [NotMapped]
    public bool isModifying { get; set; } = false;
}