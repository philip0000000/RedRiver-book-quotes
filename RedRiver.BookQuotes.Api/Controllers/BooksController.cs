using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Data;
using RedRiver.BookQuotes.Api.Models;

namespace RedRiver.BookQuotes.Api.Controllers
{
    /// <summary>
    /// Provides CRUD operations for managing book records.
    /// All endpoints require authentication via JWT.
    /// </summary>
    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns a list of all books in the database.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _db.Books.ToListAsync();  // Load all books
            return Ok(books);
        }

        /// <summary>
        /// Returns a single book by its unique identifier.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _db.Books.FindAsync(id);   // Find book by id

            if (book == null)
                return NotFound($"Book with id {id} was not found.");   // Not found

            return Ok(book);
        }

        /// <summary>
        /// Creates a new book and stores it in the database.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  // Validation failed

            // Add new book and save changes
            _db.Books.Add(request);
            await _db.SaveChangesAsync();

            // Return 201 with link
            return CreatedAtAction(nameof(GetBookById), new { id = request.Id }, request);
        }

        /// <summary>
        /// Updates an existing book with new details.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book request)
        {
            var existing = await _db.Books.FindAsync(id);   // Find book

            if (existing == null)
                return NotFound($"Book with id {id} was not found."); // Not found

            // Update relevant fields.
            existing.Title = request.Title;
            existing.Author = request.Author;
            existing.PublishedDate = request.PublishedDate;

            await _db.SaveChangesAsync();   // Save updates

            return NoContent();
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existing = await _db.Books.FindAsync(id);   // Find book

            if (existing == null)
                return NotFound($"Book with id {id} was not found.");   // Not found

            // Remove book and save change
            _db.Books.Remove(existing);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}


