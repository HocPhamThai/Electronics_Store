using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics_Store.Shared;

public class ProductVariety
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [NotMapped]
    public bool isNew { get; set; } = false;
    [NotMapped]
    public bool isModifying { get; set; } = false;
}