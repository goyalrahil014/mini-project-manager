using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ProjectManager.Api.Data;
using ProjectManager.Api.DTOs.Projects;
using ProjectManager.Api.Models;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProjectsController(AppDbContext db) { _db = db; }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _db.Projects
                .Where(p => p.UserId == CurrentUserId)
                .Select(p => new {
                    p.Id, p.Title, p.Description, p.CreatedAt
                }).ToListAsync();

            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectRequest req)
        {
            var project = new Project
            {
                Title = req.Title,
                Description = req.Description,
                UserId = CurrentUserId
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return Ok(new { project.Id, project.Title, project.Description, project.CreatedAt });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var p = await _db.Projects
                .Include(pr => pr.Tasks)
                .Where(pr => pr.UserId == CurrentUserId && pr.Id == id)
                .Select(pr => new {
                    pr.Id, pr.Title, pr.Description, pr.CreatedAt,
                    Tasks = pr.Tasks.Select(t => new { t.Id, t.Title, t.DueDate, t.IsCompleted })
                }).FirstOrDefaultAsync();

            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id && p.UserId == CurrentUserId);
            if (project == null) return NotFound();
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
