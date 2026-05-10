using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _ctx;
    public ProductRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
        => await _ctx.Products.FirstOrDefaultAsync(p => p.SKU == sku, ct);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync(ct);

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
        => await _ctx.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(ct);

    public async Task<IEnumerable<Product>> GetBySupplierAsync(Guid supplierId, CancellationToken ct = default)
        => await _ctx.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.SupplierId == supplierId)
            .ToListAsync(ct);

    public async Task<IEnumerable<Product>> GetLowStockAsync(CancellationToken ct = default)
        => await _ctx.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.Quantity <= p.LowStockThreshold && p.Quantity > 0)
            .ToListAsync(ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await _ctx.Products.AddAsync(product, ct);

    public void Update(Product product) => _ctx.Products.Update(product);
    public void Delete(Product product) => _ctx.Products.Remove(product);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _ctx;
    public CategoryRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Categories.Include(c => c.Products).ToListAsync(ct);

    public async Task AddAsync(Category category, CancellationToken ct = default)
        => await _ctx.Categories.AddAsync(category, ct);

    public void Update(Category category) => _ctx.Categories.Update(category);
    public void Delete(Category category) => _ctx.Categories.Remove(category);
}

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _ctx;
    public SupplierRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IEnumerable<Supplier>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Suppliers.Include(s => s.Products).ToListAsync(ct);

    public async Task AddAsync(Supplier supplier, CancellationToken ct = default)
        => await _ctx.Suppliers.AddAsync(supplier, ct);

    public void Update(Supplier supplier) => _ctx.Suppliers.Update(supplier);
}

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _ctx;
    public StockMovementRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<StockMovement>> GetByProductAsync(Guid productId, CancellationToken ct = default)
        => await _ctx.StockMovements
            .Include(m => m.Product)
            .Where(m => m.ProductId == productId)
            .OrderByDescending(m => m.MovementDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<StockMovement>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.StockMovements
            .Include(m => m.Product)
            .OrderByDescending(m => m.MovementDate)
            .ToListAsync(ct);

    public async Task AddAsync(StockMovement movement, CancellationToken ct = default)
        => await _ctx.StockMovements.AddAsync(movement, ct);
}
