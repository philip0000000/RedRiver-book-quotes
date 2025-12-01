namespace RedRiver.BookQuotes.Api.Dtos
{
    /// <summary>
    /// Represents the JWT issued upon successful login.
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}


