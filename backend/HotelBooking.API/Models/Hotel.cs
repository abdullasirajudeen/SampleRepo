using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    /// <summary>
    /// Represents a hotel in the booking system
    /// </summary>
    public class Hotel
    {
        /// <summary>
        /// Unique identifier for the hotel
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the hotel
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Hotel description
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Street address of the hotel
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City where the hotel is located
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Country where the hotel is located
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Hotel's star rating (1-5)
        /// </summary>
        [Range(1, 5)]
        public int StarRating { get; set; }

        /// <summary>
        /// URL to the hotel's main image
        /// </summary>
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Amenities offered by the hotel (comma-separated)
        /// </summary>
        [MaxLength(1000)]
        public string? Amenities { get; set; }

        /// <summary>
        /// Average rating based on customer reviews
        /// </summary>
        [Range(0, 5)]
        public decimal AverageRating { get; set; }

        /// <summary>
        /// Total number of reviews
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Date when the hotel was added to the system
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if the hotel is active and accepting bookings
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property for hotel's rooms
        /// </summary>
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

        /// <summary>
        /// Navigation property for hotel's reviews
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
