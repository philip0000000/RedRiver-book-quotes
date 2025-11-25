using System;

namespace RedRiver.BookQuotes.Api.Models
{
    /// <summary>
    /// Represents a book entity stored in the database.
    /// </summary>
    public class Book
    {
        // Primary key for the book record.
        public int Id { get; set; }

        // Title of the book.
        public string Title { get; set; } = string.Empty;

        // Name of the author.
        public string Author { get; set; } = string.Empty;

        // Date when the book was published.
        public DateTime PublishedDate { get; set; }
    }
}
