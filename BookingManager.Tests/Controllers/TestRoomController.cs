using BookingManager.API.Controllers;
using BookingManager.API.Models;
using BookingManager.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookingManager.Tests.Controllers;

public class TestRoomController
{
    [Fact]
    public async Task GetAll_OnSuccess_ReturnsOkResult()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetAllRooms())
            .ReturnsAsync(new List<Room>
            {
                new()
                {
                    Id = "1",
                    Name = "testRoom",
                    Description = "",
                    Capacity = 1,
                    Price = 50.00
                }
            });
        
        // Act
        var systemToTest = new RoomController(mockRoomService.Object);

        var result = (OkObjectResult)await systemToTest.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetAll_OnSuccess_InvokesRoomService()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetAllRooms())
            .ReturnsAsync(new List<Room>());

        var systemToTest = new RoomController(mockRoomService.Object);

        // Act
        var result = await systemToTest.GetAll();

        // Assert
        mockRoomService.Verify(
            service => service.GetAllRooms(),
            Times.Once()
            );
    }

    [Fact]
    public async Task GetAll_OnSuccess_ReturnsListOfRooms()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetAllRooms())
            .ReturnsAsync(new List<Room>
            {
                new()
                {
                    Id = "1",
                    Name = "testRoom",
                    Description = "",
                    Capacity = 1,
                    Price = 50.00
                }
            });

        var systemToTest = new RoomController(mockRoomService.Object);

        // Act
        var result = await systemToTest.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var resultObject = (OkObjectResult)result;
        resultObject.Value.Should().BeOfType<List<Room>>();
    }
    
    [Fact]
    public async Task GetAll_OnNoRoomsFound_Returns404()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetAllRooms())
            .ReturnsAsync(new List<Room>());

        var systemToTest = new RoomController(mockRoomService.Object);

        // Act
        var result = await systemToTest.GetAll();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var resultObject = (NotFoundResult)result;
        resultObject.StatusCode.Should().Be(404);
    }
    

    [Fact]
    public async Task Get_OnSuccess_ReturnsOkResultAsync()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetRoom("0"))
            .ReturnsAsync(new Room
            {
                Id = "0",
                Description = null,
                Capacity = 0,
                Price = 0,
                Images = null
            });

        var systemToTest = new RoomController(mockRoomService.Object);

        // Act
        var result = (OkObjectResult)await systemToTest.Get("0");

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesRoomService()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetRoom("0"))
            .ReturnsAsync(new Room
            {
                Id = "0",
                Description = null,
                Capacity = 0,
                Price = 0,
                Images = null
            });

        var systemToTest = new RoomController(mockRoomService.Object);

        // Act
        var result = (OkObjectResult)await systemToTest.Get("0");
        
        // Assert
        mockRoomService.Verify(
            service => service.GetRoom("0"),
            Times.Once()
            );
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsRoom()
    {
        // Arrange
        var mockRoomService = new Mock<IRoomService>();

        mockRoomService
            .Setup(service => service.GetRoom("0"))
            .ReturnsAsync(value: new Room
            {
                Id = "0",
                Description = null,
                Capacity = 0,
                Price = 0,
                Images = null
            });

        var systemToTest = new RoomController(mockRoomService.Object);

        // Act
        var result = (OkObjectResult)await systemToTest.Get("0");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeOfType<Room>();
    }
}