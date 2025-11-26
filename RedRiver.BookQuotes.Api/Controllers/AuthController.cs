using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Data;
using RedRiver.BookQuotes.Api.Dtos;
using RedRiver.BookQuotes.Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace RedRiver.BookQuotes.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for user authentication and account management.
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
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
    }
}
