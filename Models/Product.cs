namespace Inventory.Models;

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product(Guid id, string name, decimal price, int quantity)
    {

        if (id == Guid.Empty)
            throw new ArgumentException("Id should be a valid.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be empty.");
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be empty.");

        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}
