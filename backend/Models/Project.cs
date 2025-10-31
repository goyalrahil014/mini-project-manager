using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
