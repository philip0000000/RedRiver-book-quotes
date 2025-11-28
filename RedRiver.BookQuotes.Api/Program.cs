using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Data;
using RedRiver.BookQuotes.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RedRiver.BookQuotes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Validate JWT settings early (prevents silent startup with invalid auth)
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT key is missing from configuration.");

            builder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Ensures ASP.NET Core automatically returns 400 BadRequest
                    // when model validation fails.
                    options.SuppressModelStateInvalidFilter = false;
                });

            builder.Services.AddOpenApi();

            // Add CORS so Angular can call the API
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")   // Allow Angular dev server
                          .AllowAnyHeader()                       // Allow all headers
                          .AllowAnyMethod()                       // Allow GET, POST, PUT, DELETE
                          .AllowCredentials();                    // Allow cookies/tokens if needed
                });
            });

            // This adds the database context and connects it to the SQL Server database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Adds authentication and checks JWT tokens
            // Validates issuer, audience, lifetime, and signature
            // Uses strict token expiration
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Note: Use the standard scheme name, is safer because it avoids bugs if Microsoft updates the scheme string
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,      // Check who created the token
                        ValidateAudience = true,    // Check who the token is for
                        ValidateLifetime = true,    // Check that the token is not expired
                        ValidateIssuerSigningKey = true,    // Check the signing key

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        ),

                        ClockSkew = TimeSpan.Zero   // Strict token expiration
                    };
                });

            // Token service used by AuthController
            builder.Services.AddScoped<ITokenService, TokenService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // Enable CORS before authentication
            app.UseCors("AngularPolicy"); // Note: Must run before UseAuthentication and UseAuthorization!

            app.UseAuthentication(); // Check JWT tokens // Note: Must run before authorization!
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


