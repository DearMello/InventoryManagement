namespace InventoryManagement.Application.DTOs;

// Product
public record CreateProductDto(
    string Name,
    string SKU,
    string? Description,
    decimal UnitPrice,
    int Quantity,
    int LowStockThreshold,
    Guid CategoryId,
    Guid SupplierId
);

public record UpdateProductDto(
    string Name,
    string? Description,
    decimal UnitPrice,
    int LowStockThreshold,
    Guid CategoryId,
    Guid SupplierId
);

public record ProductDto(
    Guid Id,
    string Name,
    string SKU,
    string? Description,
    decimal UnitPrice,
    int Quantity,
    int LowStockThreshold,
    bool IsLowStock,
    string Status,
    string CategoryName,
    string SupplierName,
    DateTime CreatedAt
);

// Category
public record CreateCategoryDto(string Name, string? Description);
public record UpdateCategoryDto(string Name, string? Description);
public record CategoryDto(Guid Id, string Name, string? Description, int ProductCount);

// Supplier
public record CreateSupplierDto(string Name, string ContactEmail, string? Phone, string? Address);
public record UpdateSupplierDto(string Name, string ContactEmail, string? Phone, string? Address);
public record SupplierDto(Guid Id, string Name, string ContactEmail, string? Phone, string? Address, string Status, int ProductCount);

// Stock Movement
public record StockMovementDto(
    Guid Id,
    string ProductName,
    string SKU,
    int Quantity,
    string Type,
    string? Reason,
    DateTime MovementDate
);

public record CreateStockMovementDto(
    Guid ProductId,
    int Quantity,
    string Type,
    string? Reason
);

// Summary
public record InventorySummaryDto(
    int TotalProducts,
    int LowStockCount,
    int OutOfStockCount,
    int TotalSuppliers,
    int TotalCategories,
    decimal TotalInventoryValue
);
