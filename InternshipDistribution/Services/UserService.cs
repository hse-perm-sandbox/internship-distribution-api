using InternshipDistribution.Dto;
using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;

namespace InternshipDistribution.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly BCryptPasswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public UserService(UserRepository userRepository, BCryptPasswordHasher passwordHasher, JwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, bool isManager)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.IsManager = isManager;

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.SoftDeleteAsync(id);
        }
    }
}
