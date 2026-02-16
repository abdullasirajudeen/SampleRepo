using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new hotel
    /// </summary>
    public class CreateHotelDto
    {
        /// <summary>
        /// Name of the hotel
        /// </summary>
        [Required(ErrorMessage = "Hotel name is required")]
        [MaxLength(200, ErrorMessage = "Hotel name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Hotel description
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Street address of the hotel
        /// </summary>
        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City where the hotel is located
        /// </summary>
        [Required(ErrorMessage = "City is required")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Country where the hotel is located
        /// </summary>
        [Required(ErrorMessage = "Country is required")]
        [MaxLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Hotel's star rating (1-5)
        /// </summary>
        [Required(ErrorMessage = "Star rating is required")]
        [Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5")]
        public int StarRating { get; set; }

        /// <summary>
        /// URL to the hotel's main image
        /// </summary>
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Amenities offered by the hotel (comma-separated)
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Amenities cannot exceed 1000 characters")]
        public string? Amenities { get; set; }
    }
}
