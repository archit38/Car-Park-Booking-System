using BookingSystem.API.Configuration;
using BookingSystem.Data.Entities;
using BookingSystem.Repository;
using BookingSystem.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace ParkingService.Tests
{
    public class BookingServiceTests
    {

        private readonly CarParkConfig config = new CarParkConfig
        {
            Capacity = 10,
            WeekdayPrice = 10m,
            WeekendPrice = 15m,
            SummerPriceMultiplier = 1.2m,
            WinterPriceMultiplier = 0.8m
        };

        [Fact]
        public void CheckAvailableSpaces_PartiallyBooked_ReturnsRemainingSpaces()
        {
            // Arrange
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 1, 10);

            // Create a list of existing bookings that overlap with the provided date range
            var existingBookings = new List<Booking>
            {
                new Booking { Id = 1, FromDate = new DateTime(2023, 1, 3), ToDate = new DateTime(2023, 1, 5) },
                new Booking { Id = 2, FromDate = new DateTime(2023, 1, 7), ToDate = new DateTime(2023, 1, 8) },
            };

            var mockOptions = new Mock<IOptions<CarParkConfig>>();
            mockOptions.Setup(op => op.Value).Returns(config);

            var bookingRepositoryMock = new Mock<IBookingRepository>();
            bookingRepositoryMock.Setup(repo => repo.GetAllBookings()).Returns(existingBookings);

            var bookingService = new BookingService(bookingRepositoryMock.Object, mockOptions.Object);

            // Act
            int availableSpaces = bookingService.CheckAvailableSpaces(fromDate, toDate);

            // Assert
            Assert.Equal(8, availableSpaces);
        }

        [Fact]
        public void CheckAvailableSpaces_NoExistingBookings_ReturnsTotalSpaces()
        {
            // Arrange
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 1, 10);

            // Create an empty list of existing bookings
            var existingBookings = new List<Booking>();

            var mockOptions = new Mock<IOptions<CarParkConfig>>();
            mockOptions.Setup(op => op.Value).Returns(config);

            var bookingRepositoryMock = new Mock<IBookingRepository>();
            bookingRepositoryMock.Setup(repo => repo.GetAllBookings()).Returns(existingBookings);

            var bookingService = new BookingService(bookingRepositoryMock.Object, mockOptions.Object);

            // Act
            int availableSpaces = bookingService.CheckAvailableSpaces(fromDate, toDate);

            // Assert
            // In this scenario, there are no existing bookings, so all 10 spaces are available.
            Assert.Equal(10, availableSpaces);
        }

        [Fact]
        public void CheckAvailableSpaces_FullyBooked_ReturnsZeroSpaces()
        {
            // Arrange
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 1, 10);

            // Create a list of existing bookings that fully cover the entire date range
            var existingBookings = new List<Booking>();

            for (var currentDate = fromDate; currentDate <= toDate; currentDate = currentDate.AddDays(1))
            {
                existingBookings.Add(new Booking { FromDate = currentDate, ToDate = currentDate });
            }

            var mockOptions = new Mock<IOptions<CarParkConfig>>();
            mockOptions.Setup(op => op.Value).Returns(config);

            var bookingRepositoryMock = new Mock<IBookingRepository>();
            bookingRepositoryMock.Setup(repo => repo.GetAllBookings()).Returns(existingBookings);

            var bookingService = new BookingService(bookingRepositoryMock.Object, mockOptions.Object);

            // Act
            int availableSpaces = bookingService.CheckAvailableSpaces(fromDate, toDate);

            // Assert
            // In this scenario, the entire date range is covered by 10 existing bookings,
            // so no spaces are available.
            Assert.Equal(0, availableSpaces);
        }

    }

}
