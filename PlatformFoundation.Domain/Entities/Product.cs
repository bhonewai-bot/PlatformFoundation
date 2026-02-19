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
        Id = id;
        Apply(name, price);
    }
    
    public static Product Create(string name, decimal price) => new Product(Guid.NewGuid(), name, price);
    
    public void Update(string name, decimal price) => Apply(name, price);

    private void Apply(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("Products name is required");

        name = name.Trim();
        
        if (name.Length > 100)
            throw new DomainValidationException("Products name is too long");
        
        if (price <= 0)
            throw new DomainValidationException("Products price must be greater than 0");
        
        Name = name;
        Price = price;
    }
}
