using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedRiver.BookQuotes.Api.Data;
using RedRiver.BookQuotes.Api.Services;
using System.Text;

namespace RedRiver.BookQuotes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // --------------------------
            // Load / validate JWT config
            // --------------------------
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT key is missing from configuration.");

            // --------------------------
            // Controllers + Validation
            // --------------------------
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Ensures ASP.NET Core automatically returns 400 BadRequest
                    // when model validation fails.
                    options.SuppressModelStateInvalidFilter = false;
                });

            // --------------------------
            // OpenAPI (Swashbuckle)
            // --------------------------
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RedRiver BookQuotes API",
                    Version = "v1"
                });
            });

            // --------------------------
            // CORS — allow Angular app
            // --------------------------
            // Add CORS so Angular can call the API
            var allowedOrigins = builder.Configuration
                .GetSection("AllowedOrigins")
                .Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins!)           // Load allowed frontend URLs from configuration
                          .AllowAnyHeader()                       // Allow all headers
                          .AllowAnyMethod()                       // Allow GET, POST, PUT, DELETE
                          .AllowCredentials();                    // Allow cookies/tokens if needed
                });
            });

            // --------------------------
            // Database
            // --------------------------
            // This adds the database context and connects it to the SQL Server database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // --------------------------
            // JWT Authentication
            // --------------------------
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

            // --------------------------
            // Pipeline configuration
            // --------------------------

            // Always redirect to HTTPS on Azure
            app.UseHttpsRedirection();

            app.UseRouting(); // Important for Azure routing

            // Swagger in all environments
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookQuotes API v1");
            });

            // Enable CORS before authentication
            app.UseCors("AngularPolicy"); // Note: Must run before UseAuthentication and UseAuthorization!

            app.UseAuthentication(); // Check JWT tokens // Note: Must run before authorization!
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


