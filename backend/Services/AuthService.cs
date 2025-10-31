using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs.Auth;
using ProjectManager.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManager.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                throw new ApplicationException("Email already registered");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

            return GenerateToken(user);
        }

        private AuthResponse GenerateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwt["Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email) };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiresMinutes"]!)),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponse { Token = tokenHandler.WriteToken(token), ExpiresAt = tokenDescriptor.Expires!.Value };
        }
    }
}
