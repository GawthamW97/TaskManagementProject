using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Repository;
using TaskManagementApp.Services;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;
        private readonly IProjectRepository projectRepository;
        private readonly Services.IAuthorizationService authorizationService;
        private readonly IMapper mapper;
        private readonly ILogger<TaskController> logger;

        public TaskController(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            Services.IAuthorizationService authorizationService,
            IMapper mapper,
            ILogger<TaskController> logger)
        {
            this.taskRepository = taskRepository;
            this.projectRepository = projectRepository;
            this.authorizationService = authorizationService;
            this.mapper = mapper;
            this.logger = logger;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                   User.FindFirst("email")?.Value ?? 
                   User.FindFirst(ClaimTypes.Email)?.Value ?? "";
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectTasks([FromRoute] int projectId)
        {
            var userId = GetCurrentUserId();

            // Verify user is a project member or creator
            var project = await projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this project");
            }

            var tasks = await taskRepository.GetProjectTasksAsync(projectId);
            return Ok(mapper.Map<IReadOnlyList<GetTaskDTO>>(tasks));
        }

        [HttpGet("task/{id:int}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int id)
        {
            var userId = GetCurrentUserId();
            var task = await taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            // Verify user is a project member
            var project = await projectRepository.GetProjectByIdAsync(task.ProjectId);
            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this task");
            }

            return Ok(mapper.Map<GetTaskDTO>(task));
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskDTO request)
        {
            var userId = GetCurrentUserId();

            // Verify user is a project member
            var project = await projectRepository.GetProjectByIdAsync(request.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this project");
            }

            var task = mapper.Map<BaseTask>(request);
            task.CreatedBy = userId;
            task.UpdatedBy = userId;

            task = await taskRepository.AddTaskAsync(task);
            if (task == null)
            {
                return BadRequest("Failed to create task");
            }

            var response = mapper.Map<GetTaskDTO>(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskDTO request)
        {
            var userId = GetCurrentUserId();
            var existingTask = await taskRepository.GetTaskByIdAsync(id);

            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            // Check if task is completed and user is not admin
            if (!IsAdmin() && await authorizationService.IsTaskCompletedAsync(id))
            {
                return BadRequest("Completed tasks cannot be edited. Only comments can be added.");
            }

            // Check authorization: creator, assignee, or admin
            if (!IsAdmin() && !(existingTask.CreatedBy == userId || (existingTask.AssignedTo?.Email == userId)))
            {
                return Forbid("You do not have permission to modify this task");
            }

            var task = mapper.Map<BaseTask>(request);
            task.UpdatedBy = userId;
            task = await taskRepository.UpdateTaskAsync(id, task);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            var response = mapper.Map<GetTaskDTO>(task);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            var userId = GetCurrentUserId();
            var task = await taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            // Only task creator or admin can delete
            if (!IsAdmin() && task.CreatedBy != userId)
            {
                return Forbid("You do not have permission to delete this task");
            }

            var deletedTask = await taskRepository.DeleteTaskAsync(id);
            if (deletedTask == null)
            {
                return NotFound("Task not found");
            }

            var response = mapper.Map<GetTaskDTO>(deletedTask);
            return Ok(response);
        }

        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddComment([FromRoute] int taskId, [FromBody] AddTaskCommentDTO request)
        {
            var userId = GetCurrentUserId();
            var task = await taskRepository.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            // Verify user is a project member
            var project = await projectRepository.GetProjectByIdAsync(task.ProjectId);
            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this project");
            }

            var comment = new TaskComment
            {
                Content = request.Content,
                CreatedBy = userId,
                TaskId = taskId
            };

            var addedComment = await taskRepository.AddCommentAsync(comment);
            var response = mapper.Map<GetTaskCommentDTO>(addedComment);
            return CreatedAtAction(nameof(GetTaskComments), new { taskId = taskId }, response);
        }

        [HttpGet("{taskId}/comments")]
        public async Task<IActionResult> GetTaskComments([FromRoute] int taskId)
        {
            var userId = GetCurrentUserId();
            var task = await taskRepository.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            // Verify user is a project member
            var project = await projectRepository.GetProjectByIdAsync(task.ProjectId);
            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this project");
            }

            var comments = await taskRepository.GetTaskCommentsAsync(taskId);
            return Ok(mapper.Map<IReadOnlyList<GetTaskCommentDTO>>(comments));
        }

        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            var userId = GetCurrentUserId();
            var comments = await taskRepository.GetTaskCommentsAsync(0); // Get all to find the comment
            var comment = comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            // Only comment creator or admin can delete
            if (!IsAdmin() && comment.CreatedBy != userId)
            {
                return Forbid("You do not have permission to delete this comment");
            }

            var deletedComment = await taskRepository.DeleteCommentAsync(commentId);
            if (deletedComment == null)
            {
                return NotFound("Comment not found");
            }

            return NoContent();
        }
    }
}
