using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.Models;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Services
{
    /// <summary>
    /// Service for hotel-related business logic
    /// </summary>
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all active hotels with their rooms
        /// </summary>
        /// <returns>List of active hotels</returns>
        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.AverageRating)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific hotel by ID with related data
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>Hotel or null if not found</returns>
        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        /// <summary>
        /// Searches hotels based on criteria
        /// </summary>
        /// <param name="city">City to filter by</param>
        /// <param name="minRating">Minimum star rating</param>
        /// <param name="maxPrice">Maximum price per night</param>
        /// <returns>List of matching hotels</returns>
        public async Task<IEnumerable<Hotel>> SearchHotelsAsync(string? city, int? minRating, decimal? maxPrice)
        {
            var query = _context.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.IsActive);

            // Filter by city if provided
            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(h => h.City.ToLower().Contains(city.ToLower()));
            }

            // Filter by minimum rating if provided
            if (minRating.HasValue)
            {
                query = query.Where(h => h.StarRating >= minRating.Value);
            }

            // Filter by maximum price if provided
            if (maxPrice.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r => r.PricePerNight <= maxPrice.Value));
            }

            return await query
                .OrderByDescending(h => h.AverageRating)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new hotel
        /// </summary>
        /// <param name="createHotelDto">Hotel creation data</param>
        /// <returns>Created hotel</returns>
        public async Task<Hotel> CreateHotelAsync(CreateHotelDto createHotelDto)
        {
            var hotel = new Hotel
            {
                Name = createHotelDto.Name,
                Description = createHotelDto.Description,
                Address = createHotelDto.Address,
                City = createHotelDto.City,
                Country = createHotelDto.Country,
                StarRating = createHotelDto.StarRating,
                ImageUrl = createHotelDto.ImageUrl,
                Amenities = createHotelDto.Amenities,
                AverageRating = 0,
                ReviewCount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return hotel;
        }

        /// <summary>
        /// Updates an existing hotel
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <param name="createHotelDto">Updated hotel data</param>
        /// <returns>True if updated successfully, false if hotel not found</returns>
        public async Task<bool> UpdateHotelAsync(int id, CreateHotelDto createHotelDto)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return false;
            }

            hotel.Name = createHotelDto.Name;
            hotel.Description = createHotelDto.Description;
            hotel.Address = createHotelDto.Address;
            hotel.City = createHotelDto.City;
            hotel.Country = createHotelDto.Country;
            hotel.StarRating = createHotelDto.StarRating;
            hotel.ImageUrl = createHotelDto.ImageUrl;
            hotel.Amenities = createHotelDto.Amenities;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Soft deletes a hotel by marking it as inactive
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>True if deleted successfully, false if hotel not found</returns>
        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return false;
            }

            // Soft delete - mark as inactive instead of removing
            hotel.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets all rooms for a specific hotel
        /// </summary>
        /// <param name="hotelId">Hotel ID</param>
        /// <returns>List of rooms</returns>
        public async Task<IEnumerable<Room>> GetHotelRoomsAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.IsAvailable)
                .OrderBy(r => r.PricePerNight)
                .ToListAsync();
        }

        /// <summary>
        /// Gets available rooms for a hotel in a specific date range
        /// </summary>
        /// <param name="hotelId">Hotel ID</param>
        /// <param name="checkIn">Check-in date</param>
        /// <param name="checkOut">Check-out date</param>
        /// <returns>List of available rooms</returns>
        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            // Get all rooms for the hotel
            var allRooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.IsAvailable)
                .ToListAsync();

            // Get bookings that overlap with the requested date range
            var overlappingBookings = await _context.Bookings
                .Where(b => b.Room.HotelId == hotelId &&
                           b.Status != "Cancelled" &&
                           b.CheckInDate < checkOut &&
                           b.CheckOutDate > checkIn)
                .Select(b => b.RoomId)
                .Distinct()
                .ToListAsync();

            // Filter out booked rooms
            return allRooms.Where(r => !overlappingBookings.Contains(r.Id));
        }
    }
}
