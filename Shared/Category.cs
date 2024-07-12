using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics_Store.Shared;

public class Category
{
    public int CategoryId { get; set; }
    [NotMapped]
    public bool isEditing { get; set; } = false;
    [NotMapped]
    public bool isNew { get; set; } = false;
    public bool isViewable { get; set; } = true;
    public bool isDeleted { get; set; } = false;
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryAccessUrl { get; set; } = string.Empty;
}