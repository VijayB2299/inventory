using System.ComponentModel.DataAnnotations;

public record LoginDto
{
  [Required, EmailAddress]
  public string Email { get; init; } = String.Empty;
  [Required]
  public string Password { get; init; } = String.Empty;
}