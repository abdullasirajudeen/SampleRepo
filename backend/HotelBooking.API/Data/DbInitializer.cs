using HotelBooking.API.Models;

namespace HotelBooking.API.Data
{
    /// <summary>
    /// Initializes the database with sample data
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Seeds the database with initial data if it's empty
        /// </summary>
        /// <param name="context">Database context</param>
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Hotels.Any())
            {
                return; // Database has been seeded
            }

            // Seed Users
            var users = new User[]
            {
                new User
                {
                    Email = "admin@hotelbooking.com",
                    FullName = "Admin User",
                    PasswordHash = "hashed_password_here", // In production, use proper password hashing
                    PhoneNumber = "+1234567890",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Email = "john.doe@example.com",
                    FullName = "John Doe",
                    PasswordHash = "hashed_password_here",
                    PhoneNumber = "+1234567891",
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Email = "jane.smith@example.com",
                    FullName = "Jane Smith",
                    PasswordHash = "hashed_password_here",
                    PhoneNumber = "+1234567892",
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed Hotels
            var hotels = new Hotel[]
            {
                new Hotel
                {
                    Name = "Grand Plaza Hotel",
                    Description = "Luxury 5-star hotel in the heart of the city with stunning views and world-class amenities.",
                    Address = "123 Main Street",
                    City = "New York",
                    Country = "USA",
                    StarRating = 5,
                    ImageUrl = "https://example.com/images/grand-plaza.jpg",
                    Amenities = "WiFi,Pool,Gym,Spa,Restaurant,Bar,Room Service,Parking",
                    AverageRating = 4.5m,
                    ReviewCount = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Hotel
                {
                    Name = "Seaside Resort",
                    Description = "Beautiful beachfront resort with private beach access and water sports facilities.",
                    Address = "456 Ocean Drive",
                    City = "Miami",
                    Country = "USA",
                    StarRating = 4,
                    ImageUrl = "https://example.com/images/seaside-resort.jpg",
                    Amenities = "WiFi,Pool,Beach,Water Sports,Restaurant,Bar,Spa",
                    AverageRating = 4.3m,
                    ReviewCount = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Hotel
                {
                    Name = "Mountain View Lodge",
                    Description = "Cozy mountain lodge with breathtaking views and skiing facilities nearby.",
                    Address = "789 Alpine Road",
                    City = "Denver",
                    Country = "USA",
                    StarRating = 3,
                    ImageUrl = "https://example.com/images/mountain-lodge.jpg",
                    Amenities = "WiFi,Fireplace,Restaurant,Hiking,Skiing,Parking",
                    AverageRating = 4.0m,
                    ReviewCount = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Hotels.AddRange(hotels);
            context.SaveChanges();

            // Seed Rooms
            var rooms = new List<Room>();
            foreach (var hotel in hotels)
            {
                // Add different room types for each hotel
                rooms.AddRange(new Room[]
                {
                    new Room
                    {
                        HotelId = hotel.Id,
                        RoomNumber = "101",
                        RoomType = "Single",
                        Description = "Comfortable single room with city view",
                        PricePerNight = hotel.StarRating * 50m,
                        Capacity = 1,
                        BedCount = 1,
                        SizeInSqMeters = 20,
                        ImageUrl = "https://example.com/images/single-room.jpg",
                        Amenities = "WiFi,TV,Mini Bar,Air Conditioning",
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        HotelId = hotel.Id,
                        RoomNumber = "102",
                        RoomType = "Double",
                        Description = "Spacious double room with modern amenities",
                        PricePerNight = hotel.StarRating * 80m,
                        Capacity = 2,
                        BedCount = 1,
                        SizeInSqMeters = 30,
                        ImageUrl = "https://example.com/images/double-room.jpg",
                        Amenities = "WiFi,TV,Mini Bar,Air Conditioning,Coffee Maker",
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        HotelId = hotel.Id,
                        RoomNumber = "201",
                        RoomType = "Suite",
                        Description = "Luxury suite with separate living area",
                        PricePerNight = hotel.StarRating * 150m,
                        Capacity = 4,
                        BedCount = 2,
                        SizeInSqMeters = 60,
                        ImageUrl = "https://example.com/images/suite.jpg",
                        Amenities = "WiFi,TV,Mini Bar,Air Conditioning,Coffee Maker,Jacuzzi,Balcony",
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    }
                });
            }
            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            // Seed Reviews
            var reviews = new Review[]
            {
                new Review
                {
                    HotelId = hotels[0].Id,
                    UserId = users[1].Id,
                    Rating = 5,
                    Title = "Excellent Stay!",
                    Comment = "Amazing hotel with great service and facilities. Highly recommend!",
                    CreatedAt = DateTime.UtcNow,
                    IsVerified = true,
                    HelpfulCount = 5
                },
                new Review
                {
                    HotelId = hotels[0].Id,
                    UserId = users[2].Id,
                    Rating = 4,
                    Title = "Great Location",
                    Comment = "Perfect location in the city center. Rooms were clean and comfortable.",
                    CreatedAt = DateTime.UtcNow,
                    IsVerified = true,
                    HelpfulCount = 3
                },
                new Review
                {
                    HotelId = hotels[1].Id,
                    UserId = users[1].Id,
                    Rating = 5,
                    Title = "Paradise by the Sea",
                    Comment = "Beautiful beach resort with excellent facilities. The food was amazing!",
                    CreatedAt = DateTime.UtcNow,
                    IsVerified = true,
                    HelpfulCount = 8
                }
            };
            context.Reviews.AddRange(reviews);
            context.SaveChanges();

            // Update hotel review counts and average ratings
            foreach (var hotel in hotels)
            {
                var hotelReviews = context.Reviews.Where(r => r.HotelId == hotel.Id).ToList();
                if (hotelReviews.Any())
                {
                    hotel.ReviewCount = hotelReviews.Count;
                    hotel.AverageRating = (decimal)hotelReviews.Average(r => r.Rating);
                }
            }
            context.SaveChanges();
        }
    }
}
