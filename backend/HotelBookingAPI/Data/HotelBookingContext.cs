using HotelBookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Data;

public class HotelBookingContext : DbContext
{
    public HotelBookingContext(DbContextOptions<HotelBookingContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure Hotel entity
        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.Rooms)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.Reviews)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Room entity
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Bookings)
            .WithOne(b => b.Room)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Booking entity
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Review entity
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@hotelbooking.com",
                PasswordHash = "eY0aLY+bXMb1ll0MbeFjoYlhHZHKYPMT+OAqtC7IF0I=", // Password: Admin@123
                Role = "Admin",
                IsActive = true
            },
            new User
            {
                Id = 2,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "q8cBq4VvDRDArAGOh8KLU0QGtiNVY6JjoJQcE+T5BZ0=", // Password: User@123
                PhoneNumber = "+1234567890",
                Role = "User",
                IsActive = true
            }
        );

        // Seed hotels
        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Grand Plaza Hotel",
                Description = "Luxury 5-star hotel in the heart of the city with world-class amenities and services.",
                Address = "123 Main Street",
                City = "New York",
                Country = "USA",
                ZipCode = "10001",
                PhoneNumber = "+1234567890",
                Email = "info@grandplaza.com",
                StarRating = 5,
                ImageUrl = "https://via.placeholder.com/800x600?text=Grand+Plaza+Hotel",
                Latitude = 40.7128,
                Longitude = -74.0060,
                IsActive = true
            },
            new Hotel
            {
                Id = 2,
                Name = "Seaside Resort",
                Description = "Beautiful beachfront resort with stunning ocean views and modern facilities.",
                Address = "456 Beach Boulevard",
                City = "Miami",
                Country = "USA",
                ZipCode = "33139",
                PhoneNumber = "+1987654321",
                Email = "info@seasideresort.com",
                StarRating = 4,
                ImageUrl = "https://via.placeholder.com/800x600?text=Seaside+Resort",
                Latitude = 25.7617,
                Longitude = -80.1918,
                IsActive = true
            }
        );

        // Seed rooms
        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                Id = 1,
                HotelId = 1,
                RoomNumber = "101",
                RoomType = "Deluxe Single",
                PricePerNight = 150.00m,
                MaxOccupancy = 1,
                Description = "Comfortable single room with city view",
                ImageUrl = "https://via.placeholder.com/600x400?text=Deluxe+Single",
                HasWifi = true,
                HasAirConditioning = true,
                HasTV = true,
                HasMiniBar = true,
                IsAvailable = true
            },
            new Room
            {
                Id = 2,
                HotelId = 1,
                RoomNumber = "201",
                RoomType = "Deluxe Double",
                PricePerNight = 250.00m,
                MaxOccupancy = 2,
                Description = "Spacious double room with king-size bed",
                ImageUrl = "https://via.placeholder.com/600x400?text=Deluxe+Double",
                HasWifi = true,
                HasAirConditioning = true,
                HasTV = true,
                HasMiniBar = true,
                IsAvailable = true
            },
            new Room
            {
                Id = 3,
                HotelId = 1,
                RoomNumber = "301",
                RoomType = "Executive Suite",
                PricePerNight = 500.00m,
                MaxOccupancy = 4,
                Description = "Luxurious suite with separate living area",
                ImageUrl = "https://via.placeholder.com/600x400?text=Executive+Suite",
                HasWifi = true,
                HasAirConditioning = true,
                HasTV = true,
                HasMiniBar = true,
                IsAvailable = true
            },
            new Room
            {
                Id = 4,
                HotelId = 2,
                RoomNumber = "101",
                RoomType = "Ocean View Double",
                PricePerNight = 200.00m,
                MaxOccupancy = 2,
                Description = "Double room with stunning ocean view",
                ImageUrl = "https://via.placeholder.com/600x400?text=Ocean+View",
                HasWifi = true,
                HasAirConditioning = true,
                HasTV = true,
                HasMiniBar = false,
                IsAvailable = true
            }
        );
    }
}
