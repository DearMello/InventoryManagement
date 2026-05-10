using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(c =>
        {
            c.HasKey(x => x.Id);
            c.Property(x => x.Name).IsRequired().HasMaxLength(100);
            c.Property(x => x.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Supplier>(s =>
        {
            s.HasKey(x => x.Id);
            s.Property(x => x.Name).IsRequired().HasMaxLength(200);
            s.Property(x => x.ContactEmail).IsRequired().HasMaxLength(200);
            s.Property(x => x.Phone).HasMaxLength(30);
            s.Property(x => x.Address).HasMaxLength(400);
        });

        modelBuilder.Entity<Product>(p =>
        {
            p.HasKey(x => x.Id);
            p.Property(x => x.Name).IsRequired().HasMaxLength(200);
            p.Property(x => x.SKU).IsRequired().HasMaxLength(50);
            p.HasIndex(x => x.SKU).IsUnique();
            p.Property(x => x.Description).HasMaxLength(1000);
            p.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            p.Ignore(x => x.IsLowStock);
            p.HasOne(x => x.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
            p.HasOne(x => x.Supplier)
             .WithMany(s => s.Products)
             .HasForeignKey(x => x.SupplierId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StockMovement>(sm =>
        {
            sm.HasKey(x => x.Id);
            sm.Property(x => x.Reason).HasMaxLength(500);
            sm.HasOne(x => x.Product)
              .WithMany(p => p.StockMovements)
              .HasForeignKey(x => x.ProductId)
              .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
