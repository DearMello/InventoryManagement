using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Entities;

public class Supplier : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string ContactEmail { get; private set; } = default!;
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public SupplierStatus Status { get; private set; } = SupplierStatus.Active;

    public ICollection<Product> Products { get; private set; } = new List<Product>();

    protected Supplier() { }

    public Supplier(string name, string contactEmail, string? phone = null, string? address = null)
    {
        Name = name;
        ContactEmail = contactEmail;
        Phone = phone;
        Address = address;
    }

    public void Update(string name, string contactEmail, string? phone, string? address)
    {
        Name = name;
        ContactEmail = contactEmail;
        Phone = phone;
        Address = address;
        SetUpdated();
    }

    public void Deactivate()
    {
        Status = SupplierStatus.Inactive;
        SetUpdated();
    }

    public void Activate()
    {
        Status = SupplierStatus.Active;
        SetUpdated();
    }
}
