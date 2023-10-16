using BookingManager.API.Controllers;
using BookingManager.API.Models;
using BookingManager.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookingManager.Tests.Controllers;

public class TestBookingController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsOkResultAsync()
    {
        var mockBookingService = new Mock<IBookingService>();

        mockBookingService
            .Setup(service => service.GetAllBookings())
            .ReturnsAsync(new List<Booking>());

        var systemToTest = new BookingController(mockBookingService.Object);

        // Act
        var result = (OkObjectResult)await systemToTest.GetAllBookings();

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesBookingService()
    {
        // Arrange
        var mockBookingService = new Mock<IBookingService>();
        
        mockBookingService
            .Setup(service => service.GetAllBookings())
            .ReturnsAsync(new List<Booking>());

        var systemToTest = new BookingController(mockBookingService.Object);

        // Act
        var result = (OkObjectResult)await systemToTest.GetAllBookings();

        // Assert
        mockBookingService.Verify(
            service => service.GetAllBookings(),
            Times.Once()
            );
    }

    [Fact]
    public async Task Post_OnSuccess_InvokesBookingService()
    {
        // Arrange
        var mockBookingService = new Mock<IBookingService>();
        var mockBooking = new Booking
        {
            Email = "test@email.com",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today + TimeSpan.FromDays(1),
            NumberOfPeople = 1
        };
        mockBookingService
            .Setup(service => service.PostNewBooking(mockBooking ))
            .ReturnsAsync(new Room());
        
        var systemToTest = new BookingController(mockBookingService.Object); 
        // Act
        var result = await systemToTest.PostNewBooking(mockBooking);
        // Assert
    }
}
