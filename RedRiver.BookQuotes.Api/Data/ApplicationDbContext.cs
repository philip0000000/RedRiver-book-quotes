using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Models;

namespace RedRiver.BookQuotes.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}


