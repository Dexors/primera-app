using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PtoBackend.Models;
using PtoBackend.Services;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace PtoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel model)
        {
            try
            {
                _logger.LogInformation($"Intento de registro para el usuario: {model.Username}");
                var user = new User { Username = model.Username, Email = model.Email };
                var createdUser = await _userService.RegisterAsync(user, model.Password);
                _logger.LogInformation($"Usuario registrado exitosamente: {createdUser.Id}");
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el registro del usuario: {model.Username}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            try
            {
                _logger.LogInformation($"Intento de inicio de sesión para el usuario: {model.Username}");
                var token = await _userService.LoginAsync(model.Username, model.Password);
                _logger.LogInformation($"Inicio de sesión exitoso para el usuario: {model.Username}");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el inicio de sesión del usuario: {model.Username}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            try
            {
                _logger.LogInformation("Iniciando GetCurrentUser");
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"UserId encontrado: {userId}");
                
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Intento de acceso a /api/users/me sin ID de usuario válido");
                    return Unauthorized();
                }

                _logger.LogInformation($"Obteniendo información del usuario actual: {userId}");
                var user = await _userService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    _logger.LogWarning($"Usuario no encontrado: {userId}");
                    return NotFound();
                }

                _logger.LogInformation($"Usuario encontrado: {user.Username}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetCurrentUser");
                return StatusCode(500, "Se produjo un error interno del servidor");
            }
        }
    }

    public class RegisterModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
