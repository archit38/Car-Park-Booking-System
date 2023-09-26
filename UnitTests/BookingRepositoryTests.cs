using BookingSystem.Data.Entities;
using BookingSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace ParkingService.Tests
{
    public class BookingRepositoryTests
    {
        [Fact]
        public void GetAllBookings_ReturnsAllBookings()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CarParkDbContext(dbContextOptions))
            {
                var repository = new BookingRepository(context);

                var bookings = new List<Booking>
                {
                    new Booking { Id = 1, CustomerName = "John Doe", FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(2), Price=10m },
                    new Booking { Id = 2, CustomerName = "Jane Doe", FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(2), Price=10m }
                };
                context.Bookings.AddRange(bookings);
                context.SaveChanges();

                // Act
                var result = repository.GetAllBookings().ToList();

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Equal("John Doe", result[0].CustomerName);
                Assert.Equal("Jane Doe", result[1].CustomerName);
            }
        }

        [Fact]
        public void GetBookingById_ReturnsBooking()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CarParkDbContext(dbContextOptions))
            {
                var repository = new BookingRepository(context);

                // Add a test booking to the in-memory database
                var booking = new Booking { Id = 1, CustomerName = "John Doe", FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(2), Price = 10m };
                context.Bookings.Add(booking);
                context.SaveChanges();

                // Act
                var result = repository.GetBookingById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("John Doe", result.CustomerName);
            }
        }

        [Fact]
        public void UpdateBooking_UpdatesBookingInDatabase()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CarParkDbContext(dbContextOptions))
            {
                var repository = new BookingRepository(context);
                var booking = new Booking { CustomerName = "Initial Booking", FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(2), Price = 10m };
                context.Bookings.Add(booking);
                context.SaveChanges();

                // Act
                booking.CustomerName = "Updated Booking";
                repository.UpdateBooking(booking);

                // Assert
                var updatedBooking = context.Bookings.FirstOrDefault();
                Assert.NotNull(updatedBooking);
                Assert.Equal("Updated Booking", updatedBooking.CustomerName);
            }
        }

        [Fact]
        public void DeleteBooking_DeletesBookingFromDatabase()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CarParkDbContext(dbContextOptions))
            {
                var repository = new BookingRepository(context);
                var booking = new Booking { CustomerName = "Booking to Delete" };
                context.Bookings.Add(booking);
                context.SaveChanges();

                // Act
                repository.DeleteBooking(booking.Id);

                // Assert
                var deletedBooking = context.Bookings.FirstOrDefault(b => b.Id == booking.Id);
                Assert.Null(deletedBooking);
            }
        }
    }
}
