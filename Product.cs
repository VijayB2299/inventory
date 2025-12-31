public sealed class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    public Product(int id, string name, decimal price, int quantity)
    {

        if (id <= 0)
            throw new ArgumentException("Id should be a valid positive integer.", nameof(id));
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
