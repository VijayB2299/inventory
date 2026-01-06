
public record ProductDto
{
  public int Id { get; init; }
  public string Name { get; init; } = String.Empty;
  public decimal Price { get; init; }
  public int Quantity { get; init; }
}