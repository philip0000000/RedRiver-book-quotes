using Microsoft.IdentityModel.Tokens;
using RedRiver.BookQuotes.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RedRiver.BookQuotes.Api.Services
{
    /// <summary>
    /// Generates secure JWT tokens for authenticated users.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            // Add the user information that will be stored inside the token.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),   // User ID
                new Claim(ClaimTypes.Name, user.Username)                   // Username
            };

            // Create a security key from the secret in appsettings.json.
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            // Tell the system how to sign the token with the secret key.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Describe the token and set who it is for, who made it, and when it expires.
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),  // Token is valid for 1 hour
                signingCredentials: credentials
            );

            // Create the token string and return it.
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
