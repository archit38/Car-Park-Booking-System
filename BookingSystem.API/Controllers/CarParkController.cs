using Microsoft.AspNetCore.Mvc;
using BookingSystem.Services;
using BookingSystem.Data.Entities;
using BookingSystem.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

[Route("api/carpark/[action]")]
[ApiController]
public class CarParkController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IPricingService _pricingService;

    public CarParkController(IBookingService bookingService, IPricingService pricingService)
    {
        _bookingService = bookingService;
        _pricingService = pricingService;
    }

    [HttpGet]
    [ActionName("GetAllBookings")]
    public IActionResult GetAllBookings()
    {
        var bookings = _bookingService.GetAllBookings();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    [ActionName("GetBookingById")]
    public IActionResult GetBookingById(int id)
    {
        var booking = _bookingService.GetBookingById(id);
        if (booking == null)
        {
            return NotFound();
        }
        return Ok(booking);
    }

    /// <summary>
    /// Create a booking for a customer specifying a date range
    /// </summary>
    /// <param name="customerName"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    ///<response code="201">Returns the newly created booking Id</response>
    /// <response code="400">If there are no available spaces</response>
    [HttpPost]
    [ActionName("CreateBooking")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateBooking([FromBody] Booking booking)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (_bookingService.CheckAvailableSpaces(booking.FromDate, booking.ToDate) <= 0)
        {
            return BadRequest("Parking is not available for the specified date range.");
        }

         booking.Price = _pricingService.CalculatePrice(booking.FromDate, booking.ToDate);
        _bookingService.CreateBooking(booking);

        return CreatedAtAction("GetBookingById", new { id = booking.Id }, booking);
    }

    [HttpPut("{bookingId}")]
    [ActionName("AmendBooking")]
    public IActionResult AmendBooking(int bookingId, DateTime newFrom, DateTime newTo)
    {
        if (_bookingService.CheckAvailableSpaces(newFrom, newTo) <= 0)
        {
            return BadRequest("Parking is not available for the specified date range.");
        }

        var booking = _bookingService.GetBookingById(bookingId);
        if (booking != null)
        {
            booking.FromDate = newFrom;
            booking.ToDate = newTo;
            booking.Price = _pricingService.CalculatePrice(newFrom, newTo);
            return Ok("Booking amended successfully");
        }

        return NotFound("Booking not found");
    }

    [HttpDelete("{id}")]
    [ActionName("CancelBooking")]
    public IActionResult CancelBooking(int bookingId)
    {
        var booking = _bookingService.GetBookingById(bookingId);
        if(booking == null)
            return NotFound("Booking not found");

        _bookingService.CancelBooking(bookingId);

        return NoContent();
    }

    [HttpGet]
    [ActionName("GetPrice")]
    public IActionResult GetPrice(DateTime from, DateTime to)
    {
        decimal price = _pricingService.CalculatePrice(from, to); 

        return Ok(new { Price = price });
    }

    [HttpGet]
    [ActionName("CheckAvailability")]
    public IActionResult CheckAvailability(DateTime from, DateTime to)
    {
        int availableSpaces = _bookingService.CheckAvailableSpaces(from, to);

        return Ok(new { AvailableSpaces = availableSpaces });
    }
}

