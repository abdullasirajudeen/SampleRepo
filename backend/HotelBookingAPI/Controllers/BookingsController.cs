using HotelBookingAPI.Data;
using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly HotelBookingContext _context;

    public BookingsController(HotelBookingContext context)
    {
        _context = context;
    }

    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings([FromQuery] int? userId = null)
    {
        var query = _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(b => b.UserId == userId.Value);
        }

        var bookings = await query
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                UserName = $"{b.User.FirstName} {b.User.LastName}",
                RoomId = b.RoomId,
                RoomNumber = b.Room.RoomNumber,
                RoomType = b.Room.RoomType,
                HotelId = b.Room.HotelId,
                HotelName = b.Room.Hotel.Name,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                NumberOfGuests = b.NumberOfGuests,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                SpecialRequests = b.SpecialRequests,
                CreatedAt = b.CreatedAt
            })
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(bookings);
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found" });
        }

        var bookingDto = new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            UserName = $"{booking.User.FirstName} {booking.User.LastName}",
            RoomId = booking.RoomId,
            RoomNumber = booking.Room.RoomNumber,
            RoomType = booking.Room.RoomType,
            HotelId = booking.Room.HotelId,
            HotelName = booking.Room.Hotel.Name,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfGuests = booking.NumberOfGuests,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            SpecialRequests = booking.SpecialRequests,
            CreatedAt = booking.CreatedAt
        };

        return Ok(bookingDto);
    }

    // POST: api/Bookings
    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto createBookingDto, [FromQuery] int userId)
    {
        // Validate room exists
        var room = await _context.Rooms
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == createBookingDto.RoomId);

        if (room == null)
        {
            return NotFound(new { message = "Room not found" });
        }

        // Validate user exists
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Validate dates
        if (createBookingDto.CheckInDate < DateTime.UtcNow.Date)
        {
            return BadRequest(new { message = "Check-in date cannot be in the past" });
        }

        if (createBookingDto.CheckOutDate <= createBookingDto.CheckInDate)
        {
            return BadRequest(new { message = "Check-out date must be after check-in date" });
        }

        // Check room availability
        var conflictingBookings = await _context.Bookings
            .Where(b => b.RoomId == createBookingDto.RoomId &&
                        b.Status != "Cancelled" &&
                        ((b.CheckInDate <= createBookingDto.CheckOutDate && b.CheckOutDate >= createBookingDto.CheckInDate)))
            .AnyAsync();

        if (conflictingBookings)
        {
            return BadRequest(new { message = "Room is not available for the selected dates" });
        }

        // Calculate total price
        var numberOfNights = (createBookingDto.CheckOutDate - createBookingDto.CheckInDate).Days;
        var totalPrice = room.PricePerNight * numberOfNights;

        var booking = new Booking
        {
            UserId = userId,
            RoomId = createBookingDto.RoomId,
            CheckInDate = createBookingDto.CheckInDate,
            CheckOutDate = createBookingDto.CheckOutDate,
            NumberOfGuests = createBookingDto.NumberOfGuests,
            TotalPrice = totalPrice,
            Status = "Pending",
            SpecialRequests = createBookingDto.SpecialRequests,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        var bookingDto = new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            UserName = $"{user.FirstName} {user.LastName}",
            RoomId = booking.RoomId,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            HotelId = room.HotelId,
            HotelName = room.Hotel.Name,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfGuests = booking.NumberOfGuests,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            SpecialRequests = booking.SpecialRequests,
            CreatedAt = booking.CreatedAt
        };

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingDto);
    }

    // PUT: api/Bookings/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, UpdateBookingDto updateBookingDto)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found" });
        }

        if (booking.Status == "Cancelled")
        {
            return BadRequest(new { message = "Cannot update a cancelled booking" });
        }

        // Validate dates
        if (updateBookingDto.CheckInDate < DateTime.UtcNow.Date)
        {
            return BadRequest(new { message = "Check-in date cannot be in the past" });
        }

        if (updateBookingDto.CheckOutDate <= updateBookingDto.CheckInDate)
        {
            return BadRequest(new { message = "Check-out date must be after check-in date" });
        }

        // Check room availability (excluding current booking)
        var conflictingBookings = await _context.Bookings
            .Where(b => b.RoomId == booking.RoomId &&
                        b.Id != id &&
                        b.Status != "Cancelled" &&
                        ((b.CheckInDate <= updateBookingDto.CheckOutDate && b.CheckOutDate >= updateBookingDto.CheckInDate)))
            .AnyAsync();

        if (conflictingBookings)
        {
            return BadRequest(new { message = "Room is not available for the selected dates" });
        }

        // Calculate new total price
        var numberOfNights = (updateBookingDto.CheckOutDate - updateBookingDto.CheckInDate).Days;
        var totalPrice = booking.Room.PricePerNight * numberOfNights;

        booking.CheckInDate = updateBookingDto.CheckInDate;
        booking.CheckOutDate = updateBookingDto.CheckOutDate;
        booking.NumberOfGuests = updateBookingDto.NumberOfGuests;
        booking.TotalPrice = totalPrice;
        booking.SpecialRequests = updateBookingDto.SpecialRequests;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Bookings/5/cancel
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found" });
        }

        if (booking.Status == "Cancelled")
        {
            return BadRequest(new { message = "Booking is already cancelled" });
        }

        if (booking.Status == "Completed")
        {
            return BadRequest(new { message = "Cannot cancel a completed booking" });
        }

        booking.Status = "Cancelled";
        booking.CancelledAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Bookings/5/confirm
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found" });
        }

        if (booking.Status != "Pending")
        {
            return BadRequest(new { message = "Only pending bookings can be confirmed" });
        }

        booking.Status = "Confirmed";
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
