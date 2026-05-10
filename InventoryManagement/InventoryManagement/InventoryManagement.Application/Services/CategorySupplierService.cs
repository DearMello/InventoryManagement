using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _uow;

    public CategoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var cats = await _uow.Categories.GetAllAsync(ct);
        return cats.Select(c => new CategoryDto(c.Id, c.Name, c.Description, c.Products.Count));
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var c = await _uow.Categories.GetByIdAsync(id, ct);
        return c is null ? null : new CategoryDto(c.Id, c.Name, c.Description, c.Products.Count);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default)
    {
        var category = new Category(dto.Name, dto.Description);
        await _uow.Categories.AddAsync(category, ct);
        await _uow.SaveChangesAsync(ct);
        return new CategoryDto(category.Id, category.Name, category.Description, 0);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken ct = default)
    {
        var category = await _uow.Categories.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Category {id} not found.");

        category.Update(dto.Name, dto.Description);
        _uow.Categories.Update(category);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Categories.GetByIdAsync(id, ct);
        return new CategoryDto(updated!.Id, updated.Name, updated.Description, updated.Products.Count);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var category = await _uow.Categories.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Category {id} not found.");

        if (category.Products.Count > 0)
            throw new InvalidOperationException("Cannot delete a category that has products.");

        _uow.Categories.Delete(category);
        await _uow.SaveChangesAsync(ct);
    }
}

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _uow;

    public SupplierService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<SupplierDto>> GetAllAsync(CancellationToken ct = default)
    {
        var suppliers = await _uow.Suppliers.GetAllAsync(ct);
        return suppliers.Select(MapToDto);
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var s = await _uow.Suppliers.GetByIdAsync(id, ct);
        return s is null ? null : MapToDto(s);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken ct = default)
    {
        var supplier = new Supplier(dto.Name, dto.ContactEmail, dto.Phone, dto.Address);
        await _uow.Suppliers.AddAsync(supplier, ct);
        await _uow.SaveChangesAsync(ct);
        return MapToDto(supplier);
    }

    public async Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierDto dto, CancellationToken ct = default)
    {
        var supplier = await _uow.Suppliers.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Supplier {id} not found.");

        supplier.Update(dto.Name, dto.ContactEmail, dto.Phone, dto.Address);
        _uow.Suppliers.Update(supplier);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Suppliers.GetByIdAsync(id, ct);
        return MapToDto(updated!);
    }

    public async Task ActivateAsync(Guid id, CancellationToken ct = default)
    {
        var supplier = await _uow.Suppliers.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Supplier {id} not found.");

        supplier.Activate();
        _uow.Suppliers.Update(supplier);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var supplier = await _uow.Suppliers.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Supplier {id} not found.");

        supplier.Deactivate();
        _uow.Suppliers.Update(supplier);
        await _uow.SaveChangesAsync(ct);
    }

    private static SupplierDto MapToDto(Supplier s) => new(
        s.Id, s.Name, s.ContactEmail, s.Phone, s.Address,
        s.Status.ToString(), s.Products.Count
    );
}
