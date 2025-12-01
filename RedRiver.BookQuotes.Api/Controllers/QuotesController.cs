using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedRiver.BookQuotes.Api.Data;
using RedRiver.BookQuotes.Api.Models;
using System.Security.Claims;

namespace RedRiver.BookQuotes.Api.Controllers
{
    /// <summary>
    /// Provides CRUD actions for quote records. 
    /// User can only manage own quotes.
    /// </summary>
    [ApiController]
    [Route("api/quotes")]
    [Authorize]
    public class QuotesController : ControllerBase
    {

        private readonly ApplicationDbContext _db;

        public QuotesController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all quotes for the logged-in user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyQuotes()
        {
            // Get all quotes that belong to the user
            var userId = GetUserId();
            var quotes = await _db.Quotes
                .Where(q => q.UserId == userId)
                .ToListAsync();

            return Ok(quotes);
        }

        /// <summary>
        /// Gets one quote by id if user owns it.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuoteById(int id)
        {
            // Try to find one quote for this user
            var userId = GetUserId();
            var quote = await _db.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            if (quote == null)
                return NotFound($"Quote with id {id} not found.");  // Not found

            return Ok(quote);
        }

        /// <summary>
        /// Creates a new quote for the user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateQuote([FromBody] Quote request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);     // Validation fail

            // Create a new quote for the user
            var userId = GetUserId();
            var quote = new Quote
            {
                Text = request.Text,
                UserId = userId
            };

            // Save the new quote in the database
            _db.Quotes.Add(quote);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuoteById), new { id = quote.Id }, quote); // Return created quote info
        }

        /// <summary>
        /// Updates a quote if user owns it.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(int id, [FromBody] Quote request)
        {
            var userId = GetUserId();              // Read user id

            // Find the quote that belongs to the user
            var existing = await _db.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            // Return not found when the quote does not exist
            if (existing == null)
                return NotFound($"Quote with id {id} not found.");

            existing.Text = request.Text;          // Update fields

            await _db.SaveChangesAsync();          // Save changes
            return NoContent();
        }

        /// <summary>
        /// Deletes a quote if user owns it.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var userId = GetUserId();               // Read user id

            // Try to find the quote for this user
            var existing = await _db.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            // Return not found when quote does not exist
            if (existing == null)
                return NotFound($"Quote with id {id} not found.");

            // Remove the quote and save the change
            _db.Quotes.Remove(existing);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Reads the user id from the token.
        /// </summary>
        private int GetUserId()
        {
            // Get the user id from the token
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}


