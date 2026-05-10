using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Entities;

public class StockMovement : BaseEntity
{
    public int Quantity { get; private set; }
    public StockMovementType Type { get; private set; }
    public string? Reason { get; private set; }
    public DateTime MovementDate { get; private set; }

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    protected StockMovement() { }

    public StockMovement(int quantity, StockMovementType type, Guid productId, string? reason = null)
    {
        Quantity = quantity;
        Type = type;
        ProductId = productId;
        Reason = reason;
        MovementDate = DateTime.UtcNow;
    }
}
