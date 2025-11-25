using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Models;

namespace RedRiver.BookQuotes.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Database context that manages application data.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Table for storing books.
        public DbSet<Book> Books { get; set; }
    }
}
