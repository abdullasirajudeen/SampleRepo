using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.Models;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing rooms
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all rooms
        /// </summary>
        /// <returns>List of rooms</returns>
        /// <response code="200">Returns the list of rooms</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.IsAvailable)
                .ToListAsync();

            return Ok(rooms);
        }

        /// <summary>
        /// Gets a specific room by ID
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>Room details</returns>
        /// <response code="200">Returns the room</response>
        /// <response code="404">Room not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            return Ok(room);
        }

        /// <summary>
        /// Creates a new room
        /// </summary>
        /// <param name="room">Room data</param>
        /// <returns>Created room</returns>
        /// <response code="201">Room created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if hotel exists
            var hotelExists = await _context.Hotels.AnyAsync(h => h.Id == room.HotelId);
            if (!hotelExists)
            {
                return BadRequest(new { message = "Hotel not found" });
            }

            room.CreatedAt = DateTime.UtcNow;
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        /// <summary>
        /// Updates an existing room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <param name="room">Updated room data</param>
        /// <returns>No content</returns>
        /// <response code="204">Room updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Room not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room room)
        {
            if (id != room.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.RoomType = room.RoomType;
            existingRoom.Description = room.Description;
            existingRoom.PricePerNight = room.PricePerNight;
            existingRoom.Capacity = room.Capacity;
            existingRoom.BedCount = room.BedCount;
            existingRoom.SizeInSqMeters = room.SizeInSqMeters;
            existingRoom.ImageUrl = room.ImageUrl;
            existingRoom.Amenities = room.Amenities;
            existingRoom.IsAvailable = room.IsAvailable;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Room deleted successfully</response>
        /// <response code="404">Room not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            // Soft delete - mark as not available
            room.IsAvailable = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
