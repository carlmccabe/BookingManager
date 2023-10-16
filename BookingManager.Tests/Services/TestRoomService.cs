using BookingManager.API.Data;
using BookingManager.API.Services;
using Moq;

namespace BookingManager.Tests.Services;

public class TestRoomService
{
    [Fact]
    public async Task GetAllRooms_InvokesDbContext()
    {
        // Arrange
        var dbContextMock = new Mock<IDbContext>();
        var roomService = new RoomService(dbContextMock.Object);

        // Act
        var result = await roomService.GetAllRooms();

        // Assert
        dbContextMock.Verify(d => d.Rooms, Times.Once);
        
    }
    [Fact]
    public async Task GetRoom_InvokesDbContext()
    {
        // Arrange
        var dbContextMock = new Mock<IDbContext>();
        var roomService = new RoomService(dbContextMock.Object);

        // Act
        var result = await roomService.GetRoom("0");

        // Assert
        dbContextMock.Verify(d => d.Rooms, Times.Once);
        
    }
}