using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new booking
    /// </summary>
    public class CreateBookingDto
    {
        /// <summary>
        /// ID of the room to book
        /// </summary>
        [Required(ErrorMessage = "Room ID is required")]
        public int RoomId { get; set; }

        /// <summary>
        /// Check-in date
        /// </summary>
        [Required(ErrorMessage = "Check-in date is required")]
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-out date
        /// </summary>
        [Required(ErrorMessage = "Check-out date is required")]
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Number of guests
        /// </summary>
        [Required(ErrorMessage = "Number of guests is required")]
        [Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10")]
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Special requests or notes from the guest
        /// </summary>
        [MaxLength(500, ErrorMessage = "Special requests cannot exceed 500 characters")]
        public string? SpecialRequests { get; set; }

        /// <summary>
        /// Validates that check-out date is after check-in date
        /// </summary>
        public bool IsValid()
        {
            return CheckOutDate > CheckInDate && CheckInDate >= DateTime.UtcNow.Date;
        }
    }
}
