using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Api.DTOs.Projects
{
    public class CreateProjectRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
