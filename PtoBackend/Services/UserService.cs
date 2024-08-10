using MongoDB.Driver;
using PtoBackend.Models;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace PtoBackend.Services
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string usernameOrEmail);
        Task<User> CreateUserAsync(User user, string password);
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<User> RegisterAsync(User user, string password);
        Task<string> LoginAsync(string usernameOrEmail, string password);
        Task<User?> GetUserByIdAsync(string userId);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(IConfiguration configuration, ILogger<UserService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("La cadena de conexión de MongoDB no está configurada.");
            }
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("PtoDb");
            _users = database.GetCollection<User>("users");

            try
            {
                _users.Find(_ => true).Limit(1).ToList();
                _logger.LogInformation("Conexión a MongoDB establecida correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al conectar con MongoDB");
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(string usernameOrEmail)
        {
            return await _users.Find(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail).AnyAsync();
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            _logger.LogInformation($"Intentando crear usuario: {user.Username}, Email: {user.Email}");

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email))
            {
                _logger.LogWarning("Intento de crear usuario con nombre de usuario o correo electrónico vacío");
                throw new ArgumentException("El nombre de usuario y el correo electrónico no pueden estar vacíos.");
            }

            var existingUser = await _users.Find(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                _logger.LogWarning($"Intento de crear usuario duplicado: {user.Username}, Email: {user.Email}");
                throw new InvalidOperationException("Ya existe un usuario con ese nombre de usuario o correo electrónico.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            
            try
            {
                await _users.InsertOneAsync(user);
                _logger.LogInformation($"Usuario creado exitosamente: {user.Username}, Email: {user.Email}");
            }
            catch (MongoWriteException ex)
            {
                _logger.LogError(ex, $"Error al insertar usuario en MongoDB: {user.Username}, Email: {user.Email}");
                throw new InvalidOperationException("Error al crear el usuario en la base de datos.", ex);
            }

            return user;
        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _users.Find(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail).FirstOrDefaultAsync();
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            _logger.LogInformation($"Iniciando registro de usuario: {user.Username}, Email: {user.Email}");
            try
            {
                var createdUser = await CreateUserAsync(user, password);
                _logger.LogInformation($"Usuario registrado exitosamente: {createdUser.Username}, Email: {createdUser.Email}");
                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el registro de usuario: {user.Username}, Email: {user.Email}");
                throw;
            }
        }

        public async Task<string> LoginAsync(string usernameOrEmail, string password)
        {
            _logger.LogInformation($"Intento de inicio de sesión para: {usernameOrEmail}");
            var user = await GetUserByUsernameOrEmailAsync(usernameOrEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning($"Intento de inicio de sesión fallido para: {usernameOrEmail}");
                throw new Exception("Nombre de usuario/correo electrónico o contraseña inválidos");
            }

            _logger.LogInformation($"Inicio de sesión exitoso para: {usernameOrEmail}");
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            _logger.LogInformation($"Buscando usuario por ID: {userId}");
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                _logger.LogWarning($"Usuario no encontrado para ID: {userId}");
            }
            else
            {
                _logger.LogInformation($"Usuario encontrado: {user.Username}");
            }
            return user;
        }
    }
}
