namespace Inventory.Models;

public sealed class Role
{
  public Guid Id { get; }
  public string Name { get; }
  public ICollection<User> Users { get; } = new List<User>();

  public Role(Guid id, string name)
  {
    if (id == Guid.Empty)
      throw new ArgumentException("Id should be a valid.", nameof(id));
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name cannot be empty.", nameof(name));

    Id = id;
    Name = name;
  }
}