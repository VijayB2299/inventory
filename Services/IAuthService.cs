using Inventory.Dtos;

namespace Inventory.Services;
public interface IAuthService
{
  Task<UserDto> RegisterUser(string name, string email, string password, string roleName);
  Task<LoginResponseDto> LoginUser(string email, string password);
}