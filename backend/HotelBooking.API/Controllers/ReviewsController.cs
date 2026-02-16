using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.Models;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing reviews
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all reviews for a specific hotel
        /// </summary>
        /// <param name="hotelId">Hotel ID</param>
        /// <returns>List of reviews</returns>
        /// <response code="200">Returns the list of reviews</response>
        [HttpGet("hotel/{hotelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Review>>> GetHotelReviews(int hotelId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.HotelId == hotelId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(reviews);
        }

        /// <summary>
        /// Gets a specific review by ID
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <returns>Review details</returns>
        /// <response code="200">Returns the review</response>
        /// <response code="404">Review not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            return Ok(review);
        }

        /// <summary>
        /// Creates a new review
        /// </summary>
        /// <param name="review">Review data</param>
        /// <returns>Created review</returns>
        /// <response code="201">Review created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Review>> CreateReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if hotel exists
            var hotelExists = await _context.Hotels.AnyAsync(h => h.Id == review.HotelId);
            if (!hotelExists)
            {
                return BadRequest(new { message = "Hotel not found" });
            }

            // Check if user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == review.UserId);
            if (!userExists)
            {
                return BadRequest(new { message = "User not found" });
            }

            review.CreatedAt = DateTime.UtcNow;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Update hotel's average rating and review count
            await UpdateHotelRatingAsync(review.HotelId);

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        /// <summary>
        /// Updates an existing review
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <param name="review">Updated review data</param>
        /// <returns>No content</returns>
        /// <response code="204">Review updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Review not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            existingReview.Rating = review.Rating;
            existingReview.Title = review.Title;
            existingReview.Comment = review.Comment;

            await _context.SaveChangesAsync();

            // Update hotel's average rating
            await UpdateHotelRatingAsync(existingReview.HotelId);

            return NoContent();
        }

        /// <summary>
        /// Deletes a review
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Review deleted successfully</response>
        /// <response code="404">Review not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            var hotelId = review.HotelId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Update hotel's average rating and review count
            await UpdateHotelRatingAsync(hotelId);

            return NoContent();
        }

        /// <summary>
        /// Marks a review as helpful
        /// </summary>
        /// <param name="id">Review ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Review marked as helpful</response>
        /// <response code="404">Review not found</response>
        [HttpPut("{id}/helpful")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkHelpful(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            review.HelpfulCount++;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates hotel's average rating and review count
        /// </summary>
        /// <param name="hotelId">Hotel ID</param>
        private async Task UpdateHotelRatingAsync(int hotelId)
        {
            var hotel = await _context.Hotels.FindAsync(hotelId);
            if (hotel != null)
            {
                var reviews = await _context.Reviews
                    .Where(r => r.HotelId == hotelId)
                    .ToListAsync();

                if (reviews.Any())
                {
                    hotel.AverageRating = (decimal)reviews.Average(r => r.Rating);
                    hotel.ReviewCount = reviews.Count;
                }
                else
                {
                    hotel.AverageRating = 0;
                    hotel.ReviewCount = 0;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
