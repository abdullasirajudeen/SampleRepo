using HotelBooking.API.Models;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Services
{
    /// <summary>
    /// Interface for booking-related business logic
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Gets all bookings for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of bookings</returns>
        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);

        /// <summary>
        /// Gets a booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Booking or null if not found</returns>
        Task<Booking?> GetBookingByIdAsync(int id);

        /// <summary>
        /// Creates a new booking
        /// </summary>
        /// <param name="userId">User ID making the booking</param>
        /// <param name="createBookingDto">Booking creation data</param>
        /// <returns>Created booking</returns>
        Task<Booking> CreateBookingAsync(int userId, CreateBookingDto createBookingDto);

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="userId">User ID requesting cancellation</param>
        /// <returns>True if cancelled successfully, false otherwise</returns>
        Task<bool> CancelBookingAsync(int id, int userId);

        /// <summary>
        /// Checks if a room is available for the specified dates
        /// </summary>
        /// <param name="roomId">Room ID</param>
        /// <param name="checkIn">Check-in date</param>
        /// <param name="checkOut">Check-out date</param>
        /// <returns>True if available, false otherwise</returns>
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);

        /// <summary>
        /// Confirms a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>True if confirmed successfully, false otherwise</returns>
        Task<bool> ConfirmBookingAsync(int id);
    }
}
