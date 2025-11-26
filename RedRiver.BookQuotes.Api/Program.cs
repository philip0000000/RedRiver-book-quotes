using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Data;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

