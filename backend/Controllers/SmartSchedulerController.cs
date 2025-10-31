using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId}/schedule")]
    public class SmartSchedulerController : ControllerBase
    {
        public class TaskInput
        {
            public string Title { get; set; }
            public int EstimatedHours { get; set; }
            public DateTime DueDate { get; set; }
            public List<string> Dependencies { get; set; } = new();
        }

        public class ScheduleRequest
        {
            public List<TaskInput> Tasks { get; set; } = new();
        }

        [HttpPost]
        public IActionResult Schedule(int projectId, [FromBody] ScheduleRequest request)
        {
            var tasks = request.Tasks;

            // Perform a topological sort based on dependencies
            var sorted = new List<string>();
            var remaining = tasks.ToDictionary(t => t.Title, t => t.Dependencies.ToList());

            while (remaining.Any())
            {
                var ready = remaining
                    .Where(t => t.Value.All(dep => sorted.Contains(dep)))
                    .Select(t => t.Key)
                    .ToList();

                if (!ready.Any())
                    return BadRequest(new { error = "Cyclic dependency detected." });

                foreach (var r in ready)
                {
                    sorted.Add(r);
                    remaining.Remove(r);
                }
            }

            return Ok(new
            {
                recommendedOrder = sorted,
                message = "Tasks successfully ordered based on dependencies."
            });
        }
    }
}
