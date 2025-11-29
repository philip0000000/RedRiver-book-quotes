using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Data;
using RedRiver.BookQuotes.Api.Dtos;
using RedRiver.BookQuotes.Api.Models;
using RedRiver.BookQuotes.Api.Services;
using System.Security.Cryptography;
using System.Text;

namespace RedRiver.BookQuotes.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for user authentication and account management.
    /// All actions here are for login and register, so they do not need a token.
    /// IMPORTANT: New methods added here must also be open unless they really need auth.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenService _tokenService;

        public AuthController(ApplicationDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registers a new user by hashing and salting their password
        /// before saving the credentials to the database.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Verify that the username is unique in the database.
            var exists = await _db.Users.AnyAsync(u => u.Username == request.Username);
            if (exists)
            {
                return BadRequest("Username is already taken.");
            }

            // Create secure password hash and salt.
            using var hmac = new HMACSHA512();

            // Build the user object with hashed password.
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key
            };

            // Add the new user to the db and save new user record.
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, null); // Return a 201 status
        }

        /// <summary>
        /// Authenticates the user and issues a JWT upon successful login.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Try to find the user with the given username
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            // Recompute the hash using the stored salt
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            // Compare hashes byte-by-byte
            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid username or password.");

            // Generate JWT
            var token = _tokenService.CreateToken(user);

            return Ok(new LoginResponseDto { Token = token }); // Return the token to the client
        }
    }
}


