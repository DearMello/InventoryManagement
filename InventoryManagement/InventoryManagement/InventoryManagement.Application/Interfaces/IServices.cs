using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> GetBySupplierAsync(Guid supplierId, CancellationToken ct = default);
    Task<IEnumerable<ProductDto>> GetLowStockAsync(CancellationToken ct = default);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default);
    Task DiscontinueAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);
    Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken ct = default);
    Task<SupplierDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken ct = default);
    Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierDto dto, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
}

public interface IStockMovementService
{
    Task<IEnumerable<StockMovementDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<StockMovementDto>> GetByProductAsync(Guid productId, CancellationToken ct = default);
    Task<StockMovementDto> RegisterMovementAsync(CreateStockMovementDto dto, CancellationToken ct = default);
    Task<InventorySummaryDto> GetSummaryAsync(CancellationToken ct = default);
}
