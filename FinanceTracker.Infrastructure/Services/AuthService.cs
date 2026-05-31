using FinanceTracker.Application.DTOs.Authentication;
using FinanceTracker.Application.Exceptions;
using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _tokenGenerator;
        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = jwtTokenGenerator;
        }
        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null || !_passwordHasher.Verify(dto.Password, user.HashPassword))
                throw new AppException(
                    code: "INVALID_CREDENTIALS",
                    message: "Invalid email or password",
                    statusCode: 401);

            var token = _tokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token
            };
        }

        public async Task RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new AppException(
                    code: "EMAIL_ALREADY_EXISTS",
                    message: "Email is already registered",
                    statusCode: 409);

            var user = new User
            {
                Email = dto.Email,
                HashPassword = _passwordHasher.Hash(dto.Password),
                RegisterDate = DateTime.Now
            };

            await _userRepository.AddAsync(user);
        }
    }
}
