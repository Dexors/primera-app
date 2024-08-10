using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PtoBackend.Models;
using PtoBackend.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PtoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation($"Attempting to register user: {registerDto.Username}");

                if (await _userService.UserExistsAsync(registerDto.Username))
                {
                    _logger.LogWarning($"Registration failed: Username '{registerDto.Username}' is already taken");
                    return BadRequest(new { message = "Username is taken" });
                }

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userService.RegisterAsync(user, registerDto.Password);

                if (createdUser != null)
                {
                    _logger.LogInformation($"User registered successfully: {createdUser.Username}");
                    return Ok(new { message = "User registered successfully", userId = createdUser.Id });
                }

                _logger.LogWarning($"Failed to register user: {registerDto.Username}");
                return BadRequest(new { message = "Failed to register user" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during user registration: {registerDto.Username}");
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation($"Login attempt for user: {loginDto.Username}");

                var token = await _userService.LoginAsync(loginDto.Username, loginDto.Password);

                _logger.LogInformation($"User logged in successfully: {loginDto.Username}");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during login for user: {loginDto.Username}");
                return Unauthorized(new { message = "Invalid username or password" });
            }
        }
    }

    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
