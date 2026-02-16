namespace HotelBookingAPI.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasWifi { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool HasTV { get; set; }
    public bool HasMiniBar { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateRoomDto
{
    public int HotelId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasWifi { get; set; } = true;
    public bool HasAirConditioning { get; set; } = true;
    public bool HasTV { get; set; } = true;
    public bool HasMiniBar { get; set; } = false;
}

public class UpdateRoomDto
{
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasWifi { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool HasTV { get; set; }
    public bool HasMiniBar { get; set; }
    public bool IsAvailable { get; set; }
}

public class RoomAvailabilityDto
{
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
