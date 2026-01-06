using System.Security.Cryptography;
using Inventory.Data;
using Inventory.Dtos;
using Inventory.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class AuthService : IAuthService
{
  private readonly InventoryDbContext _dbContext;
  private readonly IJwtTokenService _jwtTokenService;
  public AuthService(InventoryDbContext dbContext, IJwtTokenService jwtTokenService)
  {
    _dbContext = dbContext;
    _jwtTokenService = jwtTokenService;
  }
  public async Task<UserDto> RegisterUser(string name, string email, string password, string roleName)
  {
    var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName) ?? throw new ArgumentException("Invalid role name.");

    // Generate salt and hash password
    byte[] salt = RandomNumberGenerator.GetBytes(16);
    var passwordHash = Convert.ToBase64String(
      KeyDerivation.Pbkdf2(
        password,
        salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 10000,
        numBytesRequested: 32
      ));

    // Instantiate user
    var user = new User(
      Guid.NewGuid(),
      name,
      email,
      passwordHash,
      salt,
      role.Id
    );

    _dbContext.Users.Add(user);
    await _dbContext.SaveChangesAsync();

    return new UserDto
    {
      Id = user.Id,
      Name = user.Name,
      Email = user.Email
    };
  }

  public async Task<LoginResponseDto> LoginUser(string email, string password)
  {
    var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email) ?? throw new ArgumentException("Invalid email or password.");

    // Verify password
    var passwordHash = Convert.ToBase64String(
      KeyDerivation.Pbkdf2(
        password,
        user.PasswordSalt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 10000,
        numBytesRequested: 32
      ));

    if (passwordHash != user.PasswordHash)
    {
      throw new ArgumentException("Invalid email or password.");
    }
  
    var token = await _jwtTokenService.GenerateToken(user);

    return new LoginResponseDto
    {
      Token = token
    };
  }
}