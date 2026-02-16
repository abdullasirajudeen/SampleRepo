using HotelBooking.API.Models;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Services
{
    /// <summary>
    /// Interface for hotel-related business logic
    /// </summary>
    public interface IHotelService
    {
        /// <summary>
        /// Gets all active hotels
        /// </summary>
        /// <returns>List of hotels</returns>
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();

        /// <summary>
        /// Gets a hotel by ID
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>Hotel or null if not found</returns>
        Task<Hotel?> GetHotelByIdAsync(int id);

        /// <summary>
        /// Searches hotels by criteria
        /// </summary>
        /// <param name="city">City to search in</param>
        /// <param name="minRating">Minimum star rating</param>
        /// <param name="maxPrice">Maximum price per night</param>
        /// <returns>List of matching hotels</returns>
        Task<IEnumerable<Hotel>> SearchHotelsAsync(string? city, int? minRating, decimal? maxPrice);

        /// <summary>
        /// Creates a new hotel
        /// </summary>
        /// <param name="createHotelDto">Hotel creation data</param>
        /// <returns>Created hotel</returns>
        Task<Hotel> CreateHotelAsync(CreateHotelDto createHotelDto);

        /// <summary>
        /// Updates an existing hotel
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <param name="createHotelDto">Updated hotel data</param>
        /// <returns>True if updated successfully, false otherwise</returns>
        Task<bool> UpdateHotelAsync(int id, CreateHotelDto createHotelDto);

        /// <summary>
        /// Deletes a hotel
        /// </summary>
        /// <param name="id">Hotel ID</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteHotelAsync(int id);

        /// <summary>
        /// Gets all rooms for a specific hotel
        /// </summary>
        /// <param name="hotelId">Hotel ID</param>
        /// <returns>List of rooms</returns>
        Task<IEnumerable<Room>> GetHotelRoomsAsync(int hotelId);

        /// <summary>
        /// Gets available rooms for a hotel in a date range
        /// </summary>
        /// <param name="hotelId">Hotel ID</param>
        /// <param name="checkIn">Check-in date</param>
        /// <param name="checkOut">Check-out date</param>
        /// <returns>List of available rooms</returns>
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut);
    }
}
