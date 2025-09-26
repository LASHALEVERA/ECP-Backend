using ECPAPI.Data;
using ECPAPI.Models;

namespace ECPAPI.Services
{
    public interface IUserService
    {
        (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string Password);
        Task<bool> CreateUserAsync(UserDTO dto);
        Task<List<UserReadOnlyDTO>> GetUsersAsync();
        Task<UserReadOnlyDTO> GetUserByUserNameAsync(string UserName);
        Task<bool> UpdateUserAsync(UserDTO dto);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<User> AuthenticateAsync(string username, string password);
    }
}
