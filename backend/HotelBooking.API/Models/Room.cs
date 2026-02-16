using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.API.Models
{
    /// <summary>
    /// Represents a room in a hotel
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Unique identifier for the room
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the hotel this room belongs to
        /// </summary>
        [Required]
        public int HotelId { get; set; }

        /// <summary>
        /// Room number or identifier
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        /// <summary>
        /// Type of room (e.g., "Single", "Double", "Suite")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the room
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Price per night for the room
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999)]
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Maximum number of guests the room can accommodate
        /// </summary>
        [Required]
        [Range(1, 10)]
        public int Capacity { get; set; }

        /// <summary>
        /// Number of beds in the room
        /// </summary>
        [Range(1, 10)]
        public int BedCount { get; set; }

        /// <summary>
        /// Room size in square meters
        /// </summary>
        [Range(0, 1000)]
        public int? SizeInSqMeters { get; set; }

        /// <summary>
        /// URL to the room's image
        /// </summary>
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Room-specific amenities (comma-separated)
        /// </summary>
        [MaxLength(500)]
        public string? Amenities { get; set; }

        /// <summary>
        /// Indicates if the room is currently available for booking
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Date when the room was added to the system
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the hotel
        /// </summary>
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; } = null!;

        /// <summary>
        /// Navigation property for room's bookings
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
