using BookingManager.API.Models;
using BookingManager.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet(Name = "GetAvailableBookings")]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllBookings();
        return Ok(bookings);
    }

    [HttpPost(Name = "PostNewBooking")]
    public async Task<IActionResult> PostNewBooking([FromBody] Booking booking)
    {
        var room = await _bookingService.PostNewBooking(booking);
        return Ok(room);
    }
}
