using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs.Tasks;
using ProjectManager.Api.Models;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private int CurrentUserId => int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);

        public TasksController(AppDbContext db) { _db = db; }

        [HttpPost("api/projects/{projectId:int}/tasks")]
        public async Task<IActionResult> CreateTask(int projectId, CreateTaskRequest req)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == CurrentUserId);
            if (project == null) return NotFound(new { message = "Project not found" });

            var t = new TaskItem { Title = req.Title, DueDate = req.DueDate, ProjectId = projectId };
            _db.Tasks.Add(t);
            await _db.SaveChangesAsync();
            return Ok(new { t.Id, t.Title, t.DueDate, t.IsCompleted });
        }

        [HttpPut("api/tasks/{taskId:int}")]
[Authorize]
public async Task<IActionResult> UpdateTask(int taskId, UpdateTaskRequest req)
{
    var task = await _db.Tasks
        .Include(t => t.Project)
        .FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == CurrentUserId);

    if (task == null)
        return NotFound(new { message = "Task not found or access denied" });

    if (string.IsNullOrWhiteSpace(req.Title))
        return BadRequest(new { message = "Title is required" });

    task.Title = req.Title;
    task.DueDate = req.DueDate;
    task.IsCompleted = req.IsCompleted;

    await _db.SaveChangesAsync();
    return Ok(task);
}


        [HttpDelete("api/tasks/{taskId:int}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _db.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == CurrentUserId);
            if (task == null) return NotFound();
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
