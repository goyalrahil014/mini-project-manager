using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
