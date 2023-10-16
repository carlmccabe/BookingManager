using BookingManager.API.Data;
using BookingManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookingManager.API.Services;

public interface IBookingService
{
    public Task<List<Booking>> GetAllBookings();
    public Task<Room?> PostNewBooking(Booking booking);
}

public class BookingService: IBookingService
{
    private IDbContext _dbContext;
    public BookingService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Booking>> GetAllBookings()
    {
        // Future Iterations: view bookings by email
        throw new NotImplementedException();
    }

    public async Task<Room?> PostNewBooking(Booking booking)
    {
        if (booking.Id == "")
            booking.Id = new Guid().ToString();
        
        var room = await _dbContext.Rooms.Find(x => x.Id == booking.RoomId).FirstOrDefaultAsync();

        if (room == null)
            return null;
        
        var isOverlap = room.Bookings != null && room.Bookings.Any(existingBooking =>
            booking.StartDate < existingBooking.EndDate &&
            booking.EndDate > existingBooking.StartDate);

        if (isOverlap)
            return null;

        room.Bookings?.Add(booking);

        var updateResult = await _dbContext.Rooms.ReplaceOneAsync(
            r => r.Id == room.Id,
            room);

        return updateResult.ModifiedCount != 1 ? null : room;
    }
}