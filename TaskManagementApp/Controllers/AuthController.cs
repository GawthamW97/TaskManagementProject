using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Repository;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository token;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository token)
        {
            this.userManager = userManager;
            this.token = token;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    var jwtToken = token.CreateJwtToken(user, roles.ToList());
                    var res = new LoginResponseDTO();
                    res.JwtToken = jwtToken;
                    return Ok(res);
                }
            }
            return BadRequest("Invalid username or password.");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult.Succeeded)
            {
                if(request.Roles != null && request.Roles.Length > 0)
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, request.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Try login now.");
                    }
                }
            }
            return BadRequest(identityResult.Errors);
        }
    }
}
