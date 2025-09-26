using AutoMapper;
using ECPAPI.Data;
using ECPAPI.Models;
using ECPAPI.Repository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ECPAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository<User> _userRepository;

        public UserService(ICategoryRepository<User> userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string Password)
        {
            byte[] salt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return (hash, Convert.ToBase64String(salt));
        }

        public async Task<bool> CreateUserAsync(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"The Argument {nameof(dto)} is null");
            var existingUser = await _userRepository.GetAsync(x => x.UserNameOrEmail.Equals(dto.UserNameOrEmail));
            if (existingUser != null)
            {
                throw new Exception("The UserNameOrEmail already taken");
            }
            User user = _mapper.Map<User>(dto);
            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                user.Password = passwordHash.PasswordHash;
                user.PasswordSalt = passwordHash.Salt;
            }
            await _userRepository.CreateAsync(user);

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException(nameof(userId));

            var existingUser = await _userRepository.GetAsync(x => !x.IsDeleted && x.Id == userId, true);
            if (existingUser == null)
            {
                throw new Exception();
            }
            existingUser.IsDeleted = true;
            await _userRepository.DeleteAsync(existingUser);
            return true;
        }

        public async Task<List<UserReadOnlyDTO>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllFilterAsync(x => !x.IsDeleted);
            return _mapper.Map<List<UserReadOnlyDTO>>(users);
        }

        public async Task<UserReadOnlyDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(x => !x.IsDeleted && x.Id == id);
            return _mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<UserReadOnlyDTO> GetUserByUserNameAsync(string UserNameOrEmail)
        {
            var user = await _userRepository.GetAsync(x => !x.IsDeleted && x.UserNameOrEmail.Equals(UserNameOrEmail));
            return _mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<bool> UpdateUserAsync(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto,nameof(dto));
            var existingUser = await _userRepository.GetAsync(x => !x.IsDeleted && x.Id == dto.Id, true);
            if (existingUser == null)
            {
                throw new Exception($"User Not Found with this id {dto.Id}");
            }

            var userToUpdate = _mapper.Map<User>(dto);
            userToUpdate.ModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                userToUpdate.Password = passwordHash.PasswordHash;
                userToUpdate.PasswordSalt = passwordHash.Salt;
            }
            await _userRepository.UpdateAsync(userToUpdate);
            return true;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(x => !x.IsDeleted && x.UserNameOrEmail == username);
            if (user == null)
                return null;

            // Verify password
            var saltBytes = Convert.FromBase64String(user.PasswordSalt);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            if (hash == user.Password)
                return user;

            return null;
        }
    }
}
