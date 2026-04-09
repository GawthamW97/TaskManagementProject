using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using TaskManagementApp.Data;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Repository;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ProjectController> logger;

        public ProjectController(IProjectRepository projectRepository, IMapper mapper, ILogger<ProjectController> logger)
        {
            this.projectRepository = projectRepository;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll Project Action method was invoked");
            var userId = GetCurrentUserId();

            // Get user's projects (where they are members or creators)
            var projects = IsAdmin() ? 
                await projectRepository.GetAllProjectsAsync() : 
                await projectRepository.GetUserProjectsAsync(userId);

            logger.LogInformation($"Retrieved projects: {JsonSerializer.Serialize(projects)}");
            return Ok(mapper.Map<IReadOnlyList<GetProjectDTO>>(projects));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var userId = GetCurrentUserId();
            var project = await projectRepository.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Check authorization: admin, project creator, or project member
            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this project");
            }

            return Ok(mapper.Map<GetProjectDTO>(project));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProjectDTO request)
        {
            var userId = GetCurrentUserId();
            var project = mapper.Map<BaseProject>(request);
            project.CreatedBy = userId;
            project.UpdatedBy = userId;

            project = await projectRepository.AddProjectAsync(project);
            if (project == null)
            {
                return BadRequest("Failed to create project");
            }

            var response = mapper.Map<GetProjectDTO>(project);
            return CreatedAtAction(nameof(GetUserById), new { id = project.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProjectDTO request)
        {
            var userId = GetCurrentUserId();
            var project = await projectRepository.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Only project creator or admin can update
            if (!IsAdmin() && project.CreatedBy != userId)
            {
                return Forbid("You do not have permission to modify this project");
            }

            var updateProject = mapper.Map<BaseProject>(request);
            updateProject.UpdatedBy = userId;
            project = await projectRepository.UpdateProjectAsync(id, updateProject);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            var response = mapper.Map<GetProjectDTO>(project);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = GetCurrentUserId();
            var project = await projectRepository.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Only project creator or admin can delete
            if (!IsAdmin() && project.CreatedBy != userId)
            {
                return Forbid("You do not have permission to delete this project");
            }

            var deletedProject = await projectRepository.DeleteProjectAsync(id);
            if (deletedProject == null)
            {
                return NotFound("Project not found");
            }

            var response = mapper.Map<GetProjectDTO>(deletedProject);
            return Ok(response);
        }

        [HttpPost("{projectId}/members/{userId}")]
        public async Task<IActionResult> AddProjectMember([FromRoute] int projectId, [FromRoute] int userId)
        {
            var currentUserId = GetCurrentUserId();
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Only project creator or admin can add members
            if (!IsAdmin() && project.CreatedBy != currentUserId)
            {
                return Forbid("You do not have permission to manage project members");
            }

            var success = await projectRepository.AddProjectMemberAsync(projectId, userId);
            if (!success)
            {
                return BadRequest("Failed to add member to project");
            }

            return Ok(new { message = "Member added successfully" });
        }

        [HttpDelete("{projectId}/members/{userId}")]
        public async Task<IActionResult> RemoveProjectMember([FromRoute] int projectId, [FromRoute] int userId)
        {
            var currentUserId = GetCurrentUserId();
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Only project creator or admin can remove members
            if (!IsAdmin() && project.CreatedBy != currentUserId)
            {
                return Forbid("You do not have permission to manage project members");
            }

            var success = await projectRepository.RemoveProjectMemberAsync(projectId, userId);
            if (!success)
            {
                return BadRequest("Failed to remove member from project");
            }

            return Ok(new { message = "Member removed successfully" });
        }

        [HttpGet("{projectId}/members")]
        public async Task<IActionResult> GetProjectMembers([FromRoute] int projectId)
        {
            var userId = GetCurrentUserId();
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Check authorization
            if (!IsAdmin() && project.CreatedBy != userId && !project.Users.Any(u => u.Email == userId))
            {
                return Forbid("You do not have access to this project");
            }

            var members = await projectRepository.GetProjectMembersAsync(projectId);
            return Ok(mapper.Map<IReadOnlyList<GetUserDTO>>(members));
        }
    }
}
