using HotelApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //ADD MODEL REFERENCES IN DATABASE
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelType> HotelTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<HotelType>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Hotel>().HasIndex("Name", "CityId").IsUnique();
            modelBuilder.Entity<Room>().HasIndex("Number", "HotelId").IsUnique();
            modelBuilder.Entity<RoomType>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Booking>().HasIndex("RoomId", "FirstDate", "LastDate").IsUnique();
           
        }
    }
}
