using System.ComponentModel.DataAnnotations;

namespace Inventory.Dtos;

public sealed class UserDto
{
  [Required]
  public required Guid Id { get; set; }
  [Required]
  public required string Name { get; set; }
  [Required]
  public required string Email { get; set; }
}