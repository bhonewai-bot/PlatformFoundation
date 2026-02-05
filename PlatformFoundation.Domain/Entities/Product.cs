namespace PlatformFoundation.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    private Product(Guid id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required", nameof(name));
        
        if (name.Length > 100)
            throw new ArgumentException("Product name is too long", nameof(name));
        
        if (price <= 0)
            throw new ArgumentException("Product price must be greater than 0.", nameof(price));
        
        Id = id;
        Name = name;
        Price = price;
    }
    
    public static Product Create(string name, decimal price) => new Product(Guid.NewGuid(), name, price);
}