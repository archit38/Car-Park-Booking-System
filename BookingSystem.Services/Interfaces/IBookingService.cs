using BookingSystem.Data.Entities;

namespace BookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        void CreateBooking(Booking booking);
        void AmendBooking(Booking booking);
        void CancelBooking(int id);
        int CheckAvailableSpaces(DateTime fromDate, DateTime toDate);
    }
}
