using PlatformFoundation.Domain.Exceptions;

namespace PlatformFoundation.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    private Product() { }

    private Product(Guid id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("Product name is required");
        
        if (name.Length > 100)
            throw new DomainValidationException("Product name is too long");
        
        if (price <= 0)
            throw new DomainValidationException("Product price must be greater than 0.");
        
        Id = id;
        Name = name;
        Price = price;
    }
    
    public static Product Create(string name, decimal price) => new Product(Guid.NewGuid(), name, price);
}
