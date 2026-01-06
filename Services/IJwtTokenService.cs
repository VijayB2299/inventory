using Inventory.Models;

public interface IJwtTokenService
{
    Task<string> GenerateToken(User user);
}