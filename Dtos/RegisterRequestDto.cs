using System.ComponentModel.DataAnnotations;

namespace Inventory.Dtos;

public record RegisterRequestDto
{
  [Required, EmailAddress]
  public string Email { get; init; } = String.Empty;
  [Required, MinLength(2)]
  public string Name { get; init; } = String.Empty;
  [Required, MinLength(6)]
  public string Password { get; init; } = String.Empty;
}