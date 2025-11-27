using RedRiver.BookQuotes.Api.Models;

namespace RedRiver.BookQuotes.Api.Services
{
    /// <summary>
    /// Defines the operations required to create JSON Web Tokens.
    /// </summary>
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}


