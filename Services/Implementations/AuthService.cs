using ECommerceAPI.DTOs.AuthDTOs;
using ECommerceAPI.Exceptions;
using ECommerceAPI.Helpers;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var exists = await _repo.EmailExistsAsync(dto.Email);

            if (exists)
                throw new BadRequestException("Email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = "User"
            };

            var hasher = new PasswordHasher<User>();

            user.PasswordHash = hasher.HashPassword(user, dto.Password);

            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            return JwtTokenHelper.GenerateToken(user, _config);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetUserByEmailAsync(dto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            return JwtTokenHelper.GenerateToken(user, _config);
        }
    }
}