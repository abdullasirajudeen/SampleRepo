using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.Models;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Services
{
    /// <summary>
    /// Service for booking-related business logic
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all bookings for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of user's bookings</returns>
        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Booking or null if not found</returns>
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Creates a new booking
        /// </summary>
        /// <param name="userId">User ID making the booking</param>
        /// <param name="createBookingDto">Booking creation data</param>
        /// <returns>Created booking</returns>
        /// <exception cref="InvalidOperationException">Thrown if room is not available or booking data is invalid</exception>
        public async Task<Booking> CreateBookingAsync(int userId, CreateBookingDto createBookingDto)
        {
            // Validate booking dates
            if (!createBookingDto.IsValid())
            {
                throw new InvalidOperationException("Invalid booking dates. Check-out must be after check-in and check-in must be in the future.");
            }

            // Check if room exists
            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == createBookingDto.RoomId);

            if (room == null)
            {
                throw new InvalidOperationException("Room not found.");
            }

            // Check if room is available for the specified dates
            var isAvailable = await IsRoomAvailableAsync(
                createBookingDto.RoomId,
                createBookingDto.CheckInDate,
                createBookingDto.CheckOutDate);

            if (!isAvailable)
            {
                throw new InvalidOperationException("Room is not available for the selected dates.");
            }

            // Check capacity
            if (createBookingDto.NumberOfGuests > room.Capacity)
            {
                throw new InvalidOperationException($"Room capacity is {room.Capacity} guests. Cannot accommodate {createBookingDto.NumberOfGuests} guests.");
            }

            // Calculate total price
            var nights = (createBookingDto.CheckOutDate - createBookingDto.CheckInDate).Days;
            var totalPrice = room.PricePerNight * nights;

            // Generate confirmation code
            var confirmationCode = GenerateConfirmationCode();

            // Create booking
            var booking = new Booking
            {
                UserId = userId,
                RoomId = createBookingDto.RoomId,
                CheckInDate = createBookingDto.CheckInDate,
                CheckOutDate = createBookingDto.CheckOutDate,
                NumberOfGuests = createBookingDto.NumberOfGuests,
                TotalPrice = totalPrice,
                Status = "Confirmed",
                SpecialRequests = createBookingDto.SpecialRequests,
                PaymentStatus = "Pending",
                BookingDate = DateTime.UtcNow,
                ConfirmationCode = confirmationCode
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="userId">User ID requesting cancellation</param>
        /// <returns>True if cancelled successfully, false otherwise</returns>
        public async Task<bool> CancelBookingAsync(int id, int userId)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null || booking.UserId != userId)
            {
                return false;
            }

            // Only allow cancellation if booking is not already cancelled
            if (booking.Status == "Cancelled")
            {
                return false;
            }

            booking.Status = "Cancelled";
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Checks if a room is available for the specified dates
        /// </summary>
        /// <param name="roomId">Room ID</param>
        /// <param name="checkIn">Check-in date</param>
        /// <param name="checkOut">Check-out date</param>
        /// <returns>True if available, false otherwise</returns>
        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            // Check if room exists and is available
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null || !room.IsAvailable)
            {
                return false;
            }

            // Check for overlapping bookings
            var hasOverlap = await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId &&
                              b.Status != "Cancelled" &&
                              b.CheckInDate < checkOut &&
                              b.CheckOutDate > checkIn);

            return !hasOverlap;
        }

        /// <summary>
        /// Confirms a booking and updates payment status
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>True if confirmed successfully, false otherwise</returns>
        public async Task<bool> ConfirmBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return false;
            }

            booking.Status = "Confirmed";
            booking.PaymentStatus = "Paid";
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Generates a unique confirmation code for a booking
        /// </summary>
        /// <returns>Confirmation code</returns>
        private string GenerateConfirmationCode()
        {
            // Generate a cryptographically secure random 8-character alphanumeric code
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new char[8];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var randomBytes = new byte[8];
            rng.GetBytes(randomBytes);
            
            for (int i = 0; i < 8; i++)
            {
                code[i] = chars[randomBytes[i] % chars.Length];
            }
            
            return new string(code);
        }
    }
}
