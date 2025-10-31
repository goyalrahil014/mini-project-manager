using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.DTOs.Tasks
{
    public class CreateTaskRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }
    }
}
