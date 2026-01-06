using System.ComponentModel.DataAnnotations;

namespace Inventory.Dtos;
public record UpdateProductPriceDto
{
  [Required]
  public decimal Price { get; set; }
}