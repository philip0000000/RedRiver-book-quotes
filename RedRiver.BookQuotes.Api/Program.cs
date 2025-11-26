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

            builder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Ensures ASP.NET Core automatically returns 400 BadRequest
                    // when model validation fails.
                    options.SuppressModelStateInvalidFilter = false;
                });

            builder.Services.AddOpenApi();

            // This adds the database context and connects it to the SQL Server database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Adds authentication support and sets up JWT validation.
            builder.Services
                .AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,      // Check who created the token
                        ValidateAudience = true,    // Check who the token is for
                        ValidateLifetime = true,    // Check that the token is not expired
                        ValidateIssuerSigningKey = true,    // Check the signing key
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        )
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

            app.UseAuthentication();    // Enables checking the user's token on each request.
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

