using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;

    public ProductService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        var products = await _uow.Products.GetAllAsync(ct);
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _uow.Products.GetByIdAsync(id, ct);
        return p is null ? null : MapToDto(p);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        var products = await _uow.Products.GetByCategoryAsync(categoryId, ct);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetBySupplierAsync(Guid supplierId, CancellationToken ct = default)
    {
        var products = await _uow.Products.GetBySupplierAsync(supplierId, ct);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockAsync(CancellationToken ct = default)
    {
        var products = await _uow.Products.GetLowStockAsync(ct);
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        var existing = await _uow.Products.GetBySkuAsync(dto.SKU, ct);
        if (existing is not null)
            throw new InvalidOperationException($"A product with SKU '{dto.SKU}' already exists.");

        var product = new Product(
            dto.Name, dto.SKU, dto.Description,
            dto.UnitPrice, dto.Quantity, dto.LowStockThreshold,
            dto.CategoryId, dto.SupplierId
        );

        await _uow.Products.AddAsync(product, ct);
        await _uow.SaveChangesAsync(ct);

        var created = await _uow.Products.GetByIdAsync(product.Id, ct);
        return MapToDto(created!);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var product = await _uow.Products.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Product {id} not found.");

        product.Update(dto.Name, dto.Description, dto.UnitPrice, dto.LowStockThreshold, dto.CategoryId, dto.SupplierId);
        _uow.Products.Update(product);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Products.GetByIdAsync(id, ct);
        return MapToDto(updated!);
    }

    public async Task DiscontinueAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _uow.Products.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Product {id} not found.");

        product.Discontinue();
        _uow.Products.Update(product);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _uow.Products.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Product {id} not found.");

        _uow.Products.Delete(product);
        await _uow.SaveChangesAsync(ct);
    }

    private static ProductDto MapToDto(Product p) => new(
        p.Id, p.Name, p.SKU, p.Description,
        p.UnitPrice, p.Quantity, p.LowStockThreshold,
        p.IsLowStock, p.Status.ToString(),
        p.Category?.Name ?? "Unknown",
        p.Supplier?.Name ?? "Unknown",
        p.CreatedAt
    );
}
