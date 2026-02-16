using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    /// <summary>
    /// Represents a user in the hotel booking system
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// User's email address (used for authentication)
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's full name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password for authentication
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// User's phone number
        /// </summary>
        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Date when the user account was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User's role in the system (e.g., "Customer", "Admin")
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Customer";

        /// <summary>
        /// Navigation property for user's bookings
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        /// <summary>
        /// Navigation property for user's reviews
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
