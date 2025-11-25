namespace RedRiver.BookQuotes.Api.Models
{
    /// <summary>
    /// Represents an application user who can log in and manage quotes.
    /// </summary>
    public class User
    {
        // Primary key for the user.
        public int Id { get; set; }

        // Unique username used for authentication.
        public string Username { get; set; } = string.Empty;

        // Secure hash of the user's password.
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    }
}
