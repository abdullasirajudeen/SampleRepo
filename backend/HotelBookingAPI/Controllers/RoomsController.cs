using HotelBookingAPI.Data;
using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly HotelBookingContext _context;

    public RoomsController(HotelBookingContext context)
    {
        _context = context;
    }

    // GET: api/Rooms
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms([FromQuery] int? hotelId = null)
    {
        var query = _context.Rooms.Include(r => r.Hotel).AsQueryable();

        if (hotelId.HasValue)
        {
            query = query.Where(r => r.HotelId == hotelId.Value);
        }

        var rooms = await query
            .Select(r => new RoomDto
            {
                Id = r.Id,
                HotelId = r.HotelId,
                HotelName = r.Hotel.Name,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                PricePerNight = r.PricePerNight,
                MaxOccupancy = r.MaxOccupancy,
                Description = r.Description,
                ImageUrl = r.ImageUrl,
                HasWifi = r.HasWifi,
                HasAirConditioning = r.HasAirConditioning,
                HasTV = r.HasTV,
                HasMiniBar = r.HasMiniBar,
                IsAvailable = r.IsAvailable
            })
            .ToListAsync();

        return Ok(rooms);
    }

    // GET: api/Rooms/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetRoom(int id)
    {
        var room = await _context.Rooms
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
        {
            return NotFound(new { message = "Room not found" });
        }

        var roomDto = new RoomDto
        {
            Id = room.Id,
            HotelId = room.HotelId,
            HotelName = room.Hotel.Name,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            PricePerNight = room.PricePerNight,
            MaxOccupancy = room.MaxOccupancy,
            Description = room.Description,
            ImageUrl = room.ImageUrl,
            HasWifi = room.HasWifi,
            HasAirConditioning = room.HasAirConditioning,
            HasTV = room.HasTV,
            HasMiniBar = room.HasMiniBar,
            IsAvailable = room.IsAvailable
        };

        return Ok(roomDto);
    }

    // GET: api/Rooms/CheckAvailability
    [HttpPost("CheckAvailability")]
    public async Task<ActionResult<bool>> CheckAvailability(RoomAvailabilityDto availabilityDto)
    {
        var conflictingBookings = await _context.Bookings
            .Where(b => b.RoomId == availabilityDto.RoomId &&
                        b.Status != "Cancelled" &&
                        ((b.CheckInDate <= availabilityDto.CheckOutDate && b.CheckOutDate >= availabilityDto.CheckInDate)))
            .AnyAsync();

        return Ok(!conflictingBookings);
    }

    // POST: api/Rooms
    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto createRoomDto)
    {
        var hotel = await _context.Hotels.FindAsync(createRoomDto.HotelId);
        if (hotel == null)
        {
            return NotFound(new { message = "Hotel not found" });
        }

        var room = new Room
        {
            HotelId = createRoomDto.HotelId,
            RoomNumber = createRoomDto.RoomNumber,
            RoomType = createRoomDto.RoomType,
            PricePerNight = createRoomDto.PricePerNight,
            MaxOccupancy = createRoomDto.MaxOccupancy,
            Description = createRoomDto.Description,
            ImageUrl = createRoomDto.ImageUrl,
            HasWifi = createRoomDto.HasWifi,
            HasAirConditioning = createRoomDto.HasAirConditioning,
            HasTV = createRoomDto.HasTV,
            HasMiniBar = createRoomDto.HasMiniBar,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        var roomDto = new RoomDto
        {
            Id = room.Id,
            HotelId = room.HotelId,
            HotelName = hotel.Name,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            PricePerNight = room.PricePerNight,
            MaxOccupancy = room.MaxOccupancy,
            Description = room.Description,
            ImageUrl = room.ImageUrl,
            HasWifi = room.HasWifi,
            HasAirConditioning = room.HasAirConditioning,
            HasTV = room.HasTV,
            HasMiniBar = room.HasMiniBar,
            IsAvailable = room.IsAvailable
        };

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, roomDto);
    }

    // PUT: api/Rooms/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, UpdateRoomDto updateRoomDto)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound(new { message = "Room not found" });
        }

        room.RoomNumber = updateRoomDto.RoomNumber;
        room.RoomType = updateRoomDto.RoomType;
        room.PricePerNight = updateRoomDto.PricePerNight;
        room.MaxOccupancy = updateRoomDto.MaxOccupancy;
        room.Description = updateRoomDto.Description;
        room.ImageUrl = updateRoomDto.ImageUrl;
        room.HasWifi = updateRoomDto.HasWifi;
        room.HasAirConditioning = updateRoomDto.HasAirConditioning;
        room.HasTV = updateRoomDto.HasTV;
        room.HasMiniBar = updateRoomDto.HasMiniBar;
        room.IsAvailable = updateRoomDto.IsAvailable;
        room.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Rooms/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound(new { message = "Room not found" });
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
