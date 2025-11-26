using System.ComponentModel.DataAnnotations;

namespace RedRiver.BookQuotes.Api.Dtos
{
    /// <summary>
    /// Represents the incoming data required to register a new user account.
    /// </summary>
    public class RegisterRequestDto
    {
        // Username of the new user.
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;

        // Plain-text password that will be hashed securely before storage.
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
