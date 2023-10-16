using BookingManager.API.Data;
using BookingManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookingManager.API.Services
{
    public interface IRoomService
    {
        public Task<List<Room>> GetAllRooms();
        Task<Room?> GetRoom(string id);
    }

    public class RoomService: IRoomService
    {
        private readonly IDbContext _dbContext;
        public RoomService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Room>> GetAllRooms()
        {
            try
            {
                var rooms = await  _dbContext.Rooms.Find(_ => true).ToListAsync();
                return rooms;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return new List<Room>();
            }
        }

        public async Task<Room?> GetRoom(string id)
        {
            try
            {
                var room = await _dbContext.Rooms.Find(x => x.Id == id ).FirstOrDefaultAsync();
                return room;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }
        }
    }
}
