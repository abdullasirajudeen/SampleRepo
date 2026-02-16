using HotelBookingAPI.Data;
using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly HotelBookingContext _context;

    public HotelsController(HotelBookingContext context)
    {
        _context = context;
    }

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels(
        [FromQuery] string? city = null,
        [FromQuery] string? country = null,
        [FromQuery] int? minStarRating = null)
    {
        var query = _context.Hotels
            .Where(h => h.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(h => h.City.ToLower().Contains(city.ToLower()));
        }

        if (!string.IsNullOrEmpty(country))
        {
            query = query.Where(h => h.Country.ToLower().Contains(country.ToLower()));
        }

        if (minStarRating.HasValue)
        {
            query = query.Where(h => h.StarRating >= minStarRating.Value);
        }

        var hotels = await query
            .Include(h => h.Reviews)
            .Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Address = h.Address,
                City = h.City,
                Country = h.Country,
                ZipCode = h.ZipCode,
                PhoneNumber = h.PhoneNumber,
                Email = h.Email,
                StarRating = h.StarRating,
                ImageUrl = h.ImageUrl,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                IsActive = h.IsActive,
                AverageRating = h.Reviews.Any() ? h.Reviews.Average(r => r.Rating) : null,
                ReviewCount = h.Reviews.Count
            })
            .ToListAsync();

        return Ok(hotels);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Reviews)
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            return NotFound(new { message = "Hotel not found" });
        }

        var hotelDto = new HotelDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Description = hotel.Description,
            Address = hotel.Address,
            City = hotel.City,
            Country = hotel.Country,
            ZipCode = hotel.ZipCode,
            PhoneNumber = hotel.PhoneNumber,
            Email = hotel.Email,
            StarRating = hotel.StarRating,
            ImageUrl = hotel.ImageUrl,
            Latitude = hotel.Latitude,
            Longitude = hotel.Longitude,
            IsActive = hotel.IsActive,
            AverageRating = hotel.Reviews.Any() ? hotel.Reviews.Average(r => r.Rating) : null,
            ReviewCount = hotel.Reviews.Count
        };

        return Ok(hotelDto);
    }

    // POST: api/Hotels
    [HttpPost]
    public async Task<ActionResult<HotelDto>> CreateHotel(CreateHotelDto createHotelDto)
    {
        var hotel = new Hotel
        {
            Name = createHotelDto.Name,
            Description = createHotelDto.Description,
            Address = createHotelDto.Address,
            City = createHotelDto.City,
            Country = createHotelDto.Country,
            ZipCode = createHotelDto.ZipCode,
            PhoneNumber = createHotelDto.PhoneNumber,
            Email = createHotelDto.Email,
            StarRating = createHotelDto.StarRating,
            ImageUrl = createHotelDto.ImageUrl,
            Latitude = createHotelDto.Latitude,
            Longitude = createHotelDto.Longitude,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        var hotelDto = new HotelDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Description = hotel.Description,
            Address = hotel.Address,
            City = hotel.City,
            Country = hotel.Country,
            ZipCode = hotel.ZipCode,
            PhoneNumber = hotel.PhoneNumber,
            Email = hotel.Email,
            StarRating = hotel.StarRating,
            ImageUrl = hotel.ImageUrl,
            Latitude = hotel.Latitude,
            Longitude = hotel.Longitude,
            IsActive = hotel.IsActive,
            AverageRating = null,
            ReviewCount = 0
        };

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotelDto);
    }

    // PUT: api/Hotels/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHotel(int id, UpdateHotelDto updateHotelDto)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
        {
            return NotFound(new { message = "Hotel not found" });
        }

        hotel.Name = updateHotelDto.Name;
        hotel.Description = updateHotelDto.Description;
        hotel.Address = updateHotelDto.Address;
        hotel.City = updateHotelDto.City;
        hotel.Country = updateHotelDto.Country;
        hotel.ZipCode = updateHotelDto.ZipCode;
        hotel.PhoneNumber = updateHotelDto.PhoneNumber;
        hotel.Email = updateHotelDto.Email;
        hotel.StarRating = updateHotelDto.StarRating;
        hotel.ImageUrl = updateHotelDto.ImageUrl;
        hotel.Latitude = updateHotelDto.Latitude;
        hotel.Longitude = updateHotelDto.Longitude;
        hotel.IsActive = updateHotelDto.IsActive;
        hotel.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
        {
            return NotFound(new { message = "Hotel not found" });
        }

        hotel.IsActive = false;
        hotel.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
