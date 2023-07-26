using BMS.Core.Configuration;
using BMS.Core.DTO;
using BMS.Core.Models;
using BMS.Repo.Data;
using BMS.Repo.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BMS.Controllers
{
    [Route("api/[controller]")] //api/authentication
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly JwtConfig _jwtConfig;

        public AuthenticationController(UserManager<User> userManager, IConfiguration configuration, IUserRepository userRepository, ApplicationDbContext applicationDbContext)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._userRepository = userRepository;
            this._applicationDbContext = applicationDbContext;
            //this._jwtConfig = jwtConfig;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Regsiter([FromBody] RegisterUserDTO registerUserDTO)
        {
            if (ModelState.IsValid)
            {
                var userExistst = await _userManager.FindByEmailAsync(registerUserDTO.Email);

                if (userExistst != null) 
                    return BadRequest("User Already exists!");

                var user = new User()
                {
                    FirstName = registerUserDTO.FirstName,
                    LastName = registerUserDTO.LastName,
                    Email = registerUserDTO.Email,
                    UserName = registerUserDTO.FirstName
                };
                var userCreated = await _userManager.CreateAsync(user, registerUserDTO.Password);

                if (userCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "UserRole");
                    var token = GenerateJwtToken(user);
                    return Ok("Registation Succesfully");
                }
                else return BadRequest("Something went wrong!");

            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(loginDTO.Email);
                //var userExists = _userRepository.checkEmailExists(loginDTO.Email);
                //var email = _applicationDbContext.Users.FirstOrDefault(x => x.Email == loginDTO.Email);

                if(userExists == null)
                {
                    return BadRequest("Please Register to our platform!");
                }

                var checkUser = _userManager.CheckPasswordAsync(userExists, loginDTO.Password);

                if (!checkUser.Result)
                {
                    return BadRequest("Invalid Credentials");
                }
                var jwtToken = GenerateJwtToken(userExists);
                return Ok(jwtToken);
            }
            return BadRequest("Invalid Credentials");
        }



        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDecriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, value:user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                }),

                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDecriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
} 
