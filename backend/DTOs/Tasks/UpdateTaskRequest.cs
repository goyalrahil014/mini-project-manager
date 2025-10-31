using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.DTOs.Tasks
{
   public class UpdateTaskRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}

}
