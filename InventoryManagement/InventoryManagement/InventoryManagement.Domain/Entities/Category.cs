using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public ICollection<Product> Products { get; private set; } = new List<Product>();

    protected Category() { }

    public Category(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        SetUpdated();
    }
}
