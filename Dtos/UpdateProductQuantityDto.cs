using System.ComponentModel.DataAnnotations;

namespace Inventory.Dtos;
public record UpdateProductQuantityDto
{
  [Required]
  public int Quantity { get; set; }
}