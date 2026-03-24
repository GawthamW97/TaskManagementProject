using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Repository;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;
        private readonly IMapper mapper;

        public TaskController(ITaskRepository taskRepository, IMapper mapper)
        {
            this.taskRepository = taskRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await taskRepository.GetAllTasksAsync();
            return Ok(mapper.Map<IReadOnlyList<GetTaskDTO>>(tasks));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int id)
        {
            var task = await taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<GetTaskDTO>(task));
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskDTO request)
        {
            var task = mapper.Map<BaseTask>(request);
            task = await taskRepository.AddTaskAsync(task);
            if (task == null)
            {
                return NotFound();
            }
            var response = mapper.Map<GetTaskDTO>(task);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskDTO request)
        {
            var task = mapper.Map<BaseTask>(request);
            task = await taskRepository.UpdateTaskAsync(id, task);
            if (task == null)
            {
                return NotFound();
            }
            var response = mapper.Map<GetTaskDTO>(task);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            var task = await taskRepository.DeleteTaskAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var response = mapper.Map<GetTaskDTO>(task);
            return Ok(response);
        }
    }
}
