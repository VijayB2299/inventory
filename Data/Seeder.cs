using System.Security.Cryptography;
using Inventory.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data;
public static class Seeder
{

  public static async Task Seed(InventoryDbContext dbContext)
  {
    await SeedRoles(dbContext);
    await SeedAdminUser(dbContext);
  }

  private static async Task SeedAdminUser(InventoryDbContext dbContext)
  {
    const string adminEmail = "admin@example.com";
    const string adminPassword = "Admin@123";

    bool IsAdminExists = await dbContext.Users
      .AnyAsync(u => u.Email == adminEmail);

    if (!IsAdminExists)
    {
      var adminRole = await dbContext.Roles
        .FirstOrDefaultAsync(r => r.Name == "Admin")
        ?? throw new Exception("Admin role not found. Please seed roles first.");

      // Generate salt and hash password
      byte[] salt = RandomNumberGenerator.GetBytes(16);
      var passwordHash = Convert.ToBase64String(
        KeyDerivation.Pbkdf2(
          adminPassword,
          salt,
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: 10000,
          numBytesRequested: 32
        ));

      var adminUser = new User(
        Guid.NewGuid(),
        "Admin User",
        adminEmail,
        passwordHash,
        salt,
        adminRole.Id
      );

      await dbContext.Users.AddAsync(adminUser);
      await dbContext.SaveChangesAsync();
    }
  }

  private static async Task SeedRoles(InventoryDbContext dbContext)
  {
    var roles = new[] { "Admin", "User" };

    foreach (var roleName in roles)
    {
      if (!await dbContext.Roles.AnyAsync(r => r.Name == roleName))
      {
        var role = new Role(Guid.NewGuid(), roleName);
        await dbContext.Roles.AddAsync(role);
      }
    }
    await dbContext.SaveChangesAsync();
  }
}