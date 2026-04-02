using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskManagementApp.Data;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Repository;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ProjectController> logger;

        public ProjectController(IProjectRepository projectRepository,IMapper mapper,ILogger<ProjectController> logger)
        {
            this.projectRepository = projectRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        // GET: api/<ProjectController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            throw new Exception("Test exception for logging");
            logger.LogInformation("GetAll Project Action method was invoked");
            var projects = await projectRepository.GetAllProjectsAsync();

            logger.LogInformation($"Retrieved all list of Projects: {JsonSerializer.Serialize(projects)}");
            return Ok(mapper.Map<IReadOnlyList<GetProjectDTO>>(projects));

        }

        // GET api/<ProjectController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            var project = await projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<GetProjectDTO>(project));
        }

        // POST api/<ProjectController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProjectDTO request)
        {
            var project = mapper.Map<BaseProject>(request);
            project = await projectRepository.AddProjectAsync(project);
            if (project == null)
            {
                return NotFound();
            }
            var response = mapper.Map<GetProjectDTO>(project);
            return Ok(response);
        }

        // PUT api/<ProjectController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProjectDTO request)
        {
            var project = mapper.Map<BaseProject>(request);
            project = await projectRepository.UpdateProjectAsync(id,project);
            if (project == null)
            {
                return NotFound();
            }
            var response = mapper.Map<GetProjectDTO>(project);
            return Ok(response);
        }

        // DELETE api/<ProjectController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var project = await projectRepository.DeleteProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var response = mapper.Map<GetProjectDTO>(project);
            return Ok(response);
        }
    }
}
