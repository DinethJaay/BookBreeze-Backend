using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">contain username and password</param>
        /// <returns>The result of the registration process</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">If the user already exists</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
            if (existingUser != null)
                return BadRequest("User already exists");

            var passwordHash = HashPassword(userDto.Password);
            var newUser = new User
            {
                Username = userDto.Username,
                PasswordHash = passwordHash
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        /// <summary>
        /// Logs in an existing user and returns a JWT token.
        /// </summary>
        /// <param name="userDto">contain username and password</param>
        /// <returns>A JWT token for the authenticated user</returns>
        /// <response code="200">User logged in successfully with a token</response>
        /// <response code="401">Invalid username or password</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>u.Username == userDto.Username);

            if (user == null || !VerifyPassword(userDto.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password");

            var token = _authService.GenerateJwtToken(user.Username);
            return Ok(new {Token = token});
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            var hash = HashPassword(password);
            return hash == passwordHash;
        }
    }
}
