namespace Inventory.Models;
public sealed class User
{
  public Guid Id { get; }
  public string Name { get; }
  public string Email { get; }
  public string PasswordHash { get; }
  public byte[] PasswordSalt { get; }
  public Guid RoleId { get; }
  public Role Role { get; } = null!;

  public User(Guid id, string name, string email, string passwordHash, byte[] passwordSalt, Guid roleId)
  {
    if (id == Guid.Empty)
      throw new ArgumentException("Id should be a valid.", nameof(id));
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name cannot be empty.", nameof(name));
    if (string.IsNullOrWhiteSpace(email))
      throw new ArgumentException("Email cannot be empty.", nameof(email));
    if (string.IsNullOrWhiteSpace(passwordHash))
      throw new ArgumentException("PasswordHash cannot be empty.", nameof(passwordHash));
    if (passwordSalt == null || passwordSalt.Length == 0)
      throw new ArgumentException("PasswordSalt cannot be empty.", nameof(passwordSalt));
    if (roleId == Guid.Empty)
      throw new ArgumentException("RoleId should be a valid.", nameof(roleId));

    Id = id;
    Name = name;
    Email = email;
    PasswordHash = passwordHash;
    PasswordSalt = passwordSalt;
    RoleId = roleId;
  }
}