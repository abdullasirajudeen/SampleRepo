using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Models;

namespace HotelBooking.API.Data
{
    /// <summary>
    /// Entity Framework Core database context for the hotel booking application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="options">Database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Hotels table
        /// </summary>
        public DbSet<Hotel> Hotels { get; set; } = null!;

        /// <summary>
        /// Rooms table
        /// </summary>
        public DbSet<Room> Rooms { get; set; } = null!;

        /// <summary>
        /// Bookings table
        /// </summary>
        public DbSet<Booking> Bookings { get; set; } = null!;

        /// <summary>
        /// Reviews table
        /// </summary>
        public DbSet<Review> Reviews { get; set; } = null!;

        /// <summary>
        /// Configure entity relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder for entity configuration</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.FullName).IsRequired();
            });

            // Configure Hotel entity
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.City);
                entity.Property(e => e.AverageRating).HasPrecision(3, 2);
            });

            // Configure Room entity
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasOne(r => r.Hotel)
                      .WithMany(h => h.Rooms)
                      .HasForeignKey(r => r.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.HotelId, e.RoomNumber }).IsUnique();
            });

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.User)
                      .WithMany(u => u.Bookings)
                      .HasForeignKey(b => b.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Room)
                      .WithMany(r => r.Bookings)
                      .HasForeignKey(b => b.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.ConfirmationCode).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RoomId);
            });

            // Configure Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasOne(r => r.Hotel)
                      .WithMany(h => h.Reviews)
                      .HasForeignKey(r => r.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.HotelId);
                entity.HasIndex(e => e.UserId);
            });
        }
    }
}
