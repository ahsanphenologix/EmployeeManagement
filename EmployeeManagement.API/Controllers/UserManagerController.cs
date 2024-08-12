using EmployeeManagement.API.Models.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _userRoleManager;
        private readonly IConfiguration _configuration;

        public UserManagerController(UserManager<ApplicationUser> userManager, IConfiguration configuration,
            RoleManager<IdentityRole> userRoleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userRoleManager = userRoleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            try
            {
                var users = _userManager.Users.ToList();

                if(users.Any())
                    return Ok(users);

                return NoContent();    
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("GetAllUserRoles")]
        public async Task<IActionResult> GetAllUserRoles(string name)
        {
            try
            {
                var user = _userManager.Users.
                    Where(usr => usr.Name == name).FirstOrDefault();

                if(user == null)
                    return NoContent();

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any())
                    return Ok(roles);

                return NoContent();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateNewRole")]
        public async Task<IActionResult> CreateNewRole([FromBody] string name)
        {
            try
            {
                var user = _userManager.Users.
                    Where(usr => usr.Name == name).FirstOrDefault();

                if(user == null)
                    return NoContent();

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any())
                    return Ok(roles);

                return NoContent();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
