namespace InventoryManagement.Domain.Enums;

public enum StockMovementType
{
    StockIn = 1,
    StockOut = 2,
    Adjustment = 3,
    Return = 4
}

public enum SupplierStatus
{
    Active = 1,
    Inactive = 2
}

public enum ProductStatus
{
    Active = 1,
    Discontinued = 2,
    OutOfStock = 3
}
