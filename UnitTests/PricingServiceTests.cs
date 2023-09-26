using BookingSystem.API.Configuration;
using BookingSystem.Services.Implementations;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace ParkingService.Tests
{
    //Add tests for booking service and controller
    public class PricingServiceTests
    {
        private readonly CarParkConfig config = new CarParkConfig
        {
            WeekdayPrice = 10m,
            WeekendPrice = 15m,
            SummerPriceMultiplier = 1.2m,
            WinterPriceMultiplier = 0.8m
        };

        [Theory]
        [InlineData("2024-07-01", "2024-07-05", 48)]  // Weekday summer
        [InlineData("2024-01-13", "2024-01-14", 12)]  // Weekend winter
        [InlineData("2023-09-12", "2023-09-15", 30)]  // Weekday, no specific season
        [InlineData("2024-03-16", "2024-03-17", 15)]  // Weekend, no specific season
        [InlineData("2024-06-30", "2024-07-01", 18)]  // Edge case: Sunday during summer
        [InlineData("2024-07-06", "2024-07-07", 18)]  // Weekend during summer
        [InlineData("2024-01-06", "2024-01-07", 12)]  // Weekend during winter
        public void CalculatePrice_ReturnsCorrectPrice(string fromDate, string toDate, decimal expectedPrice)
        {
            // Arrange

            var mockOptions = new Mock<IOptions<CarParkConfig>>();
            mockOptions.Setup(op => op.Value).Returns(config);

            var pricingService = new PricingService(mockOptions.Object);
            var from = DateTime.Parse(fromDate);
            var to = DateTime.Parse(toDate);

            // Act
            var price = pricingService.CalculatePrice(from, to);

            // Assert
            Assert.Equal(expectedPrice, price);
        }
    }
}
