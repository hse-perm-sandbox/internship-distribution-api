using InternshipDistribution.Dto;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using Microsoft.AspNetCore.Identity;

namespace InternshipDistribution.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly BCryptPasswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public AuthService(UserRepository userRepository, BCryptPasswordHasher passwordHasher, JwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<User> Register(RegisterDto registerDto)
        {
            if (await _userRepository.GetUserByEmail(registerDto.Email) != null)
                throw new BadHttpRequestException($"Пользователь с Email {registerDto.Email} уже существует");

            // Хеширование пароля
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
                IsManager = registerDto.IsManager
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null)
                throw new BadHttpRequestException($"Email {loginDto.Email} не зарегестрирован", StatusCodes.Status404NotFound);

            if (!_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                throw new BadHttpRequestException("Пароль неверный", StatusCodes.Status401Unauthorized);

            return _jwtService.GenerateToken(user);
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
    }
}
