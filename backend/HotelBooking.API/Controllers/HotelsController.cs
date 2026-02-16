using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.Models;
using HotelBooking.API.Services;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing hotels
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="hotelService">Hotel service</param>
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Gets all hotels
        /// </summary>
        /// <returns>List of hotels</returns>
        /// <response code="200">Returns the list of hotels</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        /// <summary>
        /// Gets a specific hotel by ID
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>Hotel details</returns>
        /// <response code="200">Returns the hotel</response>
        /// <response code="404">Hotel not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            return Ok(hotel);
        }

        /// <summary>
        /// Searches hotels based on criteria
        /// </summary>
        /// <param name="city">City to search in</param>
        /// <param name="minRating">Minimum star rating</param>
        /// <param name="maxPrice">Maximum price per night</param>
        /// <returns>List of matching hotels</returns>
        /// <response code="200">Returns the list of matching hotels</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Hotel>>> SearchHotels(
            [FromQuery] string? city,
            [FromQuery] int? minRating,
            [FromQuery] decimal? maxPrice)
        {
            var hotels = await _hotelService.SearchHotelsAsync(city, minRating, maxPrice);
            return Ok(hotels);
        }

        /// <summary>
        /// Creates a new hotel
        /// </summary>
        /// <param name="createHotelDto">Hotel creation data</param>
        /// <returns>Created hotel</returns>
        /// <response code="201">Hotel created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Hotel>> CreateHotel([FromBody] CreateHotelDto createHotelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = await _hotelService.CreateHotelAsync(createHotelDto);
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        /// <summary>
        /// Updates an existing hotel
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <param name="createHotelDto">Updated hotel data</param>
        /// <returns>No content</returns>
        /// <response code="204">Hotel updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Hotel not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] CreateHotelDto createHotelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _hotelService.UpdateHotelAsync(id, createHotelDto);
            if (!result)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a hotel
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Hotel deleted successfully</response>
        /// <response code="404">Hotel not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var result = await _hotelService.DeleteHotelAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Gets all rooms for a specific hotel
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>List of rooms</returns>
        /// <response code="200">Returns the list of rooms</response>
        [HttpGet("{id}/rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Room>>> GetHotelRooms(int id)
        {
            var rooms = await _hotelService.GetHotelRoomsAsync(id);
            return Ok(rooms);
        }

        /// <summary>
        /// Gets available rooms for a hotel in a date range
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <param name="checkIn">Check-in date</param>
        /// <param name="checkOut">Check-out date</param>
        /// <returns>List of available rooms</returns>
        /// <response code="200">Returns the list of available rooms</response>
        [HttpGet("{id}/available-rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms(
            int id,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            var rooms = await _hotelService.GetAvailableRoomsAsync(id, checkIn, checkOut);
            return Ok(rooms);
        }
    }
}
