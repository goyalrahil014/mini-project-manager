namespace ProjectManager.Api.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
