using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Dtos.Auth;
using TaskFlow.Api.Interfaces.Auth;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<User> userManager,
            ITokenService tokenService,
            SignInManager<User> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user is null)
            {
                return Unauthorized("Email or password incorrect.");
            }

            var validatePassword = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!validatePassword.Succeeded)
            {
                return Unauthorized("Email or password incorrect.");
            }

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                return BadRequest("Email is already in use.");
            }

            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            var createUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (!createUser.Succeeded)
            {
                return StatusCode(500, createUser.Errors);
            }

            return Ok("User created.");
        }
    }
}
