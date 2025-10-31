using ProjectManager.Api.DTOs.Auth;

namespace ProjectManager.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}
