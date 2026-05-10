using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Application.Services;

public class StockMovementService : IStockMovementService
{
    private readonly IUnitOfWork _uow;

    public StockMovementService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<StockMovementDto>> GetAllAsync(CancellationToken ct = default)
    {
        var movements = await _uow.StockMovements.GetAllAsync(ct);
        return movements.Select(MapToDto);
    }

    public async Task<IEnumerable<StockMovementDto>> GetByProductAsync(Guid productId, CancellationToken ct = default)
    {
        var movements = await _uow.StockMovements.GetByProductAsync(productId, ct);
        return movements.Select(MapToDto);
    }

    public async Task<StockMovementDto> RegisterMovementAsync(CreateStockMovementDto dto, CancellationToken ct = default)
    {
        if (!Enum.TryParse<StockMovementType>(dto.Type, true, out var movementType))
            throw new InvalidOperationException($"Invalid movement type: {dto.Type}");

        var product = await _uow.Products.GetByIdAsync(dto.ProductId, ct)
            ?? throw new KeyNotFoundException($"Product {dto.ProductId} not found.");

        // Calculate quantity delta based on movement type
        var delta = movementType switch
        {
            StockMovementType.StockIn => dto.Quantity,
            StockMovementType.Return => dto.Quantity,
            StockMovementType.StockOut => -dto.Quantity,
            StockMovementType.Adjustment => dto.Quantity, // can be positive or negative
            _ => throw new InvalidOperationException("Unknown movement type.")
        };

        if (product.Quantity + delta < 0)
            throw new InvalidOperationException(
                $"Insufficient stock. Current: {product.Quantity}, requested out: {dto.Quantity}");

        var movement = new StockMovement(dto.Quantity, movementType, dto.ProductId, dto.Reason);
        await _uow.StockMovements.AddAsync(movement, ct);

        product.AdjustQuantity(delta);
        _uow.Products.Update(product);

        await _uow.SaveChangesAsync(ct);

        var created = (await _uow.StockMovements.GetByProductAsync(dto.ProductId, ct))
            .First(m => m.Id == movement.Id);
        return MapToDto(created);
    }

    public async Task<InventorySummaryDto> GetSummaryAsync(CancellationToken ct = default)
    {
        var products = (await _uow.Products.GetAllAsync(ct)).ToList();
        var suppliers = await _uow.Suppliers.GetAllAsync(ct);
        var categories = await _uow.Categories.GetAllAsync(ct);

        return new InventorySummaryDto(
            TotalProducts: products.Count,
            LowStockCount: products.Count(p => p.IsLowStock),
            OutOfStockCount: products.Count(p => p.Quantity == 0),
            TotalSuppliers: suppliers.Count(),
            TotalCategories: categories.Count(),
            TotalInventoryValue: products.Sum(p => p.UnitPrice * p.Quantity)
        );
    }

    private static StockMovementDto MapToDto(StockMovement m) => new(
        m.Id,
        m.Product?.Name ?? "Unknown",
        m.Product?.SKU ?? "Unknown",
        m.Quantity,
        m.Type.ToString(),
        m.Reason,
        m.MovementDate
    );
}
