using System.ComponentModel.DataAnnotations;

namespace RedRiver.BookQuotes.Api.Dtos
{
    /// <summary>
    /// Represents the required login credentials for user authentication.
    /// </summary>
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}


