using System.ComponentModel.DataAnnotations;

public record CreateProductDto
{
    [Required]
    public string Name { get; init; } = String.Empty;
    [Range(0, double.MaxValue)]
    public decimal Price { get; init; }
    [Range(0, int.MaxValue)]
    public int Quantity { get; init; }
}