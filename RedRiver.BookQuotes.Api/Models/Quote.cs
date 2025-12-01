namespace RedRiver.BookQuotes.Api.Models
{
    /// <summary>
    /// Represents a quote added by a specific user.
    /// </summary>
    public class Quote
    {
        // Primary key for the quote.
        public int Id { get; set; }

        // Identifier of the user who created the quote.
        public int UserId { get; set; }

        // The text content of the quote.
        public string Text { get; set; } = string.Empty;
    }
}


