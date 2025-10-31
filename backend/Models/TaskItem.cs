using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
