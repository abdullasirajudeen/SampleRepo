using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.Models;
using HotelBooking.API.Services;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing bookings
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="bookingService">Booking service</param>
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Gets all bookings for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of bookings</returns>
        /// <response code="200">Returns the list of bookings</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetUserBookings(int userId)
        {
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }

        /// <summary>
        /// Gets a specific booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Booking details</returns>
        /// <response code="200">Returns the booking</response>
        /// <response code="404">Booking not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            return Ok(booking);
        }

        /// <summary>
        /// Creates a new booking
        /// </summary>
        /// <param name="userId">User ID making the booking</param>
        /// <param name="createBookingDto">Booking creation data</param>
        /// <returns>Created booking</returns>
        /// <response code="201">Booking created successfully</response>
        /// <response code="400">Invalid input data or room not available</response>
        [HttpPost("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Booking>> CreateBooking(
            int userId,
            [FromBody] CreateBookingDto createBookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var booking = await _bookingService.CreateBookingAsync(userId, createBookingDto);
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="userId">User ID requesting cancellation</param>
        /// <returns>No content</returns>
        /// <response code="204">Booking cancelled successfully</response>
        /// <response code="404">Booking not found or unauthorized</response>
        [HttpDelete("{id}/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelBooking(int id, int userId)
        {
            var result = await _bookingService.CancelBookingAsync(id, userId);
            if (!result)
            {
                return NotFound(new { message = "Booking not found or unauthorized to cancel" });
            }

            return NoContent();
        }

        /// <summary>
        /// Checks room availability
        /// </summary>
        /// <param name="roomId">Room ID</param>
        /// <param name="checkIn">Check-in date</param>
        /// <param name="checkOut">Check-out date</param>
        /// <returns>Availability status</returns>
        /// <response code="200">Returns availability status</response>
        [HttpGet("check-availability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> CheckAvailability(
            [FromQuery] int roomId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            var isAvailable = await _bookingService.IsRoomAvailableAsync(roomId, checkIn, checkOut);
            return Ok(new { roomId, checkIn, checkOut, isAvailable });
        }

        /// <summary>
        /// Confirms a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Booking confirmed successfully</response>
        /// <response code="404">Booking not found</response>
        [HttpPut("{id}/confirm")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var result = await _bookingService.ConfirmBookingAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Booking not found" });
            }

            return NoContent();
        }
    }
}
