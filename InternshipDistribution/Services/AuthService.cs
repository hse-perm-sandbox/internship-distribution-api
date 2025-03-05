using InternshipDistribution.Dto;
using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;
using NuGet.Protocol.Plugins;

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

        public async Task<SignUpResponse> Register(RegisterInput registerDto)
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

            await _userRepository.AddAsync(user);

            return new SignUpResponse
            {
                Id = user.Id,
                Email = user.Email,
                IsManager = user.IsManager
            };
        }

        public async Task<string> Login(LoginInput loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                throw new BadHttpRequestException($"Email {loginDto.Email} не зарегестрирован или пароль неверный", StatusCodes.Status401Unauthorized);

            return _jwtService.GenerateToken(user);
        }
    }
}
