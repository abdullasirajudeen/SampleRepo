using HotelBookingAPI.Data;
using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly HotelBookingContext _context;

    public ReviewsController(HotelBookingContext context)
    {
        _context = context;
    }

    // GET: api/Reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews([FromQuery] int? hotelId = null, [FromQuery] int? userId = null)
    {
        var query = _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Hotel)
            .Where(r => r.IsApproved)
            .AsQueryable();

        if (hotelId.HasValue)
        {
            query = query.Where(r => r.HotelId == hotelId.Value);
        }

        if (userId.HasValue)
        {
            query = query.Where(r => r.UserId == userId.Value);
        }

        var reviews = await query
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                HotelId = r.HotelId,
                HotelName = r.Hotel.Name,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                IsApproved = r.IsApproved
            })
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return Ok(reviews);
    }

    // GET: api/Reviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReview(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            UserName = $"{review.User.FirstName} {review.User.LastName}",
            HotelId = review.HotelId,
            HotelName = review.Hotel.Name,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            IsApproved = review.IsApproved
        };

        return Ok(reviewDto);
    }

    // POST: api/Reviews
    [HttpPost]
    public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto createReviewDto, [FromQuery] int userId)
    {
        // Validate hotel exists
        var hotel = await _context.Hotels.FindAsync(createReviewDto.HotelId);
        if (hotel == null)
        {
            return NotFound(new { message = "Hotel not found" });
        }

        // Validate user exists
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Check if user has already reviewed this hotel
        var existingReview = await _context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.HotelId == createReviewDto.HotelId);

        if (existingReview != null)
        {
            return BadRequest(new { message = "You have already reviewed this hotel" });
        }

        var review = new Review
        {
            UserId = userId,
            HotelId = createReviewDto.HotelId,
            Rating = createReviewDto.Rating,
            Title = createReviewDto.Title,
            Comment = createReviewDto.Comment,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            UserName = $"{user.FirstName} {user.LastName}",
            HotelId = review.HotelId,
            HotelName = hotel.Name,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            IsApproved = review.IsApproved
        };

        return CreatedAtAction(nameof(GetReview), new { id = review.Id }, reviewDto);
    }

    // PUT: api/Reviews/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, UpdateReviewDto updateReviewDto, [FromQuery] int userId)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        if (review.UserId != userId)
        {
            return Forbid();
        }

        review.Rating = updateReviewDto.Rating;
        review.Title = updateReviewDto.Title;
        review.Comment = updateReviewDto.Comment;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Reviews/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id, [FromQuery] int userId)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        if (review.UserId != userId)
        {
            return Forbid();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Reviews/5/approve
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        review.IsApproved = true;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
