
using BookingSystem.Data.Entities;
using BookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ParkingService.Tests
{
    public class CarParkControllerTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly CarParkController _controller;

        public CarParkControllerTests()
        {
            _controller = new CarParkController(_bookingServiceMock.Object, _pricingServiceMock.Object);
        }
        [Fact]
        public void CreateBooking_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var mockBookingService = new Mock<IBookingService>();
            var mockPricingService = new Mock<IPricingService>();
            var controller = new CarParkController(mockBookingService.Object, mockPricingService.Object);

            // Ensure the model is invalid by not populating required fields
            var invalidModel = new Booking();

            // Act
            var result = controller.CreateBooking(invalidModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateBooking_ValidModel_ReturnsCreatedAtAction()
        {
            var fromDate = DateTime.Today;
            var toDate = DateTime.Today.AddDays(1);
            // Arrange
            var mockBookingService = new Mock<IBookingService>();
            mockBookingService.Setup(service => service.CheckAvailableSpaces(fromDate, toDate)).Returns(5);

            var mockPricingService = new Mock<IPricingService>();
            var controller = new CarParkController(mockBookingService.Object, mockPricingService.Object);

            // Create a valid BookingModel with required properties populated
            var validModel = new Booking
            {
                CustomerName = "John Doe",
                FromDate = fromDate,
                ToDate = toDate
            };

            // Act
            var result = controller.CreateBooking(validModel);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
    // Add similar tests for AmendBooking, CancelBooking, GetPrice, and CheckAvailability
}
