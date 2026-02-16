using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.API.Models
{
    /// <summary>
    /// Represents a customer review for a hotel
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unique identifier for the review
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the hotel being reviewed
        /// </summary>
        [Required]
        public int HotelId { get; set; }

        /// <summary>
        /// Foreign key to the user who wrote the review
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Rating given by the user (1-5)
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        /// <summary>
        /// Review title/summary
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed review comment
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Date when the review was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if the review has been verified
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Number of users who found this review helpful
        /// </summary>
        public int HelpfulCount { get; set; } = 0;

        /// <summary>
        /// Navigation property to the hotel
        /// </summary>
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; } = null!;

        /// <summary>
        /// Navigation property to the user
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
