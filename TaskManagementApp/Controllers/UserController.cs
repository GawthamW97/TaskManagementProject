using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Class;
using TaskManagementApp.Models.DomainModels;
using TaskManagementApp.Models.DTOs;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserRepository UserRepository { get; }
        public IMapper mapper { get; }

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            UserRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await UserRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var user = await UserRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO user)
        {

            // Map the DTO to Domain Model
            var userDomainModel = mapper.Map<BaseUser>(user);

            // Create a new region domain model
            userDomainModel = await UserRepository.AddUserAsync(userDomainModel);

            //Map the domain model to DTO
            var userDTO = mapper.Map<GetUserDTO>(userDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = userDTO.Id }, userDTO);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDTO user)
        {
            var userDomainModel = mapper.Map<BaseUser>(user);
            var updatedUser = await UserRepository.UpdateUserAsync(id, userDomainModel);
            if (updatedUser == null)
            {
                return NotFound();
            }
            var userDTO = mapper.Map<GetUserDTO>(updatedUser);
            return Ok(userDTO);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var deletedUser = await UserRepository.DeleteUserAsync(id);
            if (deletedUser == null)
            {
                return NotFound();
            }
            var userDTO = mapper.Map<GetUserDTO>(deletedUser);
            return Ok(userDTO);

        }
    }
}
