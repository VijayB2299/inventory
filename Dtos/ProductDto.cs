
public record ProductDto
{
  public Guid Id { get; init; }
  public string Name { get; init; } = String.Empty;
  public decimal Price { get; init; }
  public int Quantity { get; init; }
}