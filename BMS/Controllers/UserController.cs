using BMS.Repo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        public UserController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpDelete]
        [Route("DeletePost")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromBody] string id = null, string email = null)
        {
            if (id ==null)
            {
                var userbyEmail = _userRepository.checkEmailExists(email);
                if(userbyEmail != null)
                {
                    _userRepository.Remove(userbyEmail);
                    _userRepository.Save();
                    return Ok("User Deleted");
                }
                return BadRequest();
            }
            var user = _userRepository.GetUserById(id);
            _userRepository.Remove(user);
            _userRepository.Save();
            return Ok("User Deleted");
        }

    }
}
