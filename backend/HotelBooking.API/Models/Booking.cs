using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.API.Models
{
    /// <summary>
    /// Represents a booking in the hotel system
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Unique identifier for the booking
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the user who made the booking
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Foreign key to the room being booked
        /// </summary>
        [Required]
        public int RoomId { get; set; }

        /// <summary>
        /// Check-in date
        /// </summary>
        [Required]
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-out date
        /// </summary>
        [Required]
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Number of guests
        /// </summary>
        [Required]
        [Range(1, 10)]
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Total price for the booking
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Current status of the booking
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Special requests or notes from the guest
        /// </summary>
        [MaxLength(500)]
        public string? SpecialRequests { get; set; }

        /// <summary>
        /// Payment status
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        /// <summary>
        /// Date when the booking was created
        /// </summary>
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the booking was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Confirmation code for the booking
        /// </summary>
        [MaxLength(20)]
        public string? ConfirmationCode { get; set; }

        /// <summary>
        /// Navigation property to the user
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to the room
        /// </summary>
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } = null!;
    }
}
