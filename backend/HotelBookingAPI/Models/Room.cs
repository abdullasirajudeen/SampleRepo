using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingAPI.Models;

public class Room
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int HotelId { get; set; }

    [Required]
    [StringLength(100)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string RoomType { get; set; } = string.Empty; // Single, Double, Suite, Deluxe, etc.

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerNight { get; set; }

    [Range(1, 10)]
    public int MaxOccupancy { get; set; } = 2;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(1000)]
    public string? ImageUrl { get; set; }

    public bool HasWifi { get; set; } = true;

    public bool HasAirConditioning { get; set; } = true;

    public bool HasTV { get; set; } = true;

    public bool HasMiniBar { get; set; } = false;

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("HotelId")]
    public virtual Hotel Hotel { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
