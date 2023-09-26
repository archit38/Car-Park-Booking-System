using BookingSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace BookingSystem.Repository
{
    public class CarParkDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        public CarParkDbContext(DbContextOptions<CarParkDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(b =>
            {
                b.HasKey(e => e.Id);
                b.HasIndex(b => new { b.FromDate, b.ToDate })
                    .IsUnique(false);
            });
        }
    }

}
