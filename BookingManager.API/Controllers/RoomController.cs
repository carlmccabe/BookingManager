using BookingManager.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("all",Name = "GetAllRooms")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var rooms = await _roomService.GetAllRooms();

            if (rooms.Any())
                return Ok(rooms);
            
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in API");
        }
    }

    [HttpGet("{id}", Name = "GetRoom")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var room = await _roomService.GetRoom(id);

            return Ok(room);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in API");
        } 
    }
}
