using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Data;
using InventoryManagement.Infrastructure.Repositories;

namespace InventoryManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;

    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }
    public ISupplierRepository Suppliers { get; }
    public IStockMovementRepository StockMovements { get; }

    public UnitOfWork(AppDbContext ctx)
    {
        _ctx = ctx;
        Products = new ProductRepository(ctx);
        Categories = new CategoryRepository(ctx);
        Suppliers = new SupplierRepository(ctx);
        StockMovements = new StockMovementRepository(ctx);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);
}
