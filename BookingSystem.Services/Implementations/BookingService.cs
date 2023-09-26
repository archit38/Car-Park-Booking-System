using BookingSystem.API.Configuration;
using BookingSystem.Data.Entities;
using BookingSystem.Repository;
using BookingSystem.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly CarParkConfig _config;

        public BookingService(IBookingRepository bookingRepository, IOptions<CarParkConfig> config)
        {
            _bookingRepository = bookingRepository;
            _config = config.Value;
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }

        public Booking GetBookingById(int id)
        {
            return _bookingRepository.GetBookingById(id);
        }

        public void CreateBooking(Booking booking)
        {
            _bookingRepository.AddBooking(booking);
        }

        public void AmendBooking(Booking booking)
        {
            _bookingRepository.UpdateBooking(booking);
        }

        public void CancelBooking(int id)
        {
            _bookingRepository.DeleteBooking(id);
        }

        public int CheckAvailableSpaces(DateTime fromDate, DateTime toDate)
        {
            var existingBookings = _bookingRepository.GetAllBookings()
                .Where(b => b.FromDate <= toDate && b.ToDate >= fromDate)
                .ToList();

            return _config.Capacity - existingBookings.Count;
        }
    }
}
