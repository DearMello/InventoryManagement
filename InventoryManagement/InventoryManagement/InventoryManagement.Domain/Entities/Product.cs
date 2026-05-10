using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string SKU { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public int LowStockThreshold { get; private set; }
    public ProductStatus Status { get; private set; } = ProductStatus.Active;

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;

    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;

    public ICollection<StockMovement> StockMovements { get; private set; } = new List<StockMovement>();

    protected Product() { }

    public Product(string name, string sku, string? description, decimal unitPrice,
        int quantity, int lowStockThreshold, Guid categoryId, Guid supplierId)
    {
        Name = name;
        SKU = sku;
        Description = description;
        UnitPrice = unitPrice;
        Quantity = quantity;
        LowStockThreshold = lowStockThreshold;
        CategoryId = categoryId;
        SupplierId = supplierId;
        Status = quantity > 0 ? ProductStatus.Active : ProductStatus.OutOfStock;
    }

    public void Update(string name, string? description, decimal unitPrice, int lowStockThreshold, Guid categoryId, Guid supplierId)
    {
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
        LowStockThreshold = lowStockThreshold;
        CategoryId = categoryId;
        SupplierId = supplierId;
        SetUpdated();
    }

    public void AdjustQuantity(int delta)
    {
        Quantity += delta;
        Status = Quantity > 0 ? ProductStatus.Active : ProductStatus.OutOfStock;
        SetUpdated();
    }

    public bool IsLowStock => Quantity <= LowStockThreshold && Quantity > 0;

    public void Discontinue()
    {
        Status = ProductStatus.Discontinued;
        SetUpdated();
    }
}
