using BookingManager.API.Models;
using MongoDB.Driver;

namespace BookingManager.API.Data
{
    public interface IDbContext
    {
        IMongoCollection<Room> Rooms { get; }
        void SeedDataIfNecessary();
    }

    public class DbContext : IDbContext
    {
        private readonly IMongoDatabase _database;

        public DbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("DbSettings:ConnectionString").Value;
            var databaseName = configuration.GetSection("DbSettings:DatabaseName").Value;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Room> Rooms => _database.GetCollection<Room>("Rooms");

        public void SeedDataIfNecessary()
        {
            if (!_database.ListCollectionNames().ToList().Contains("Rooms"))
            {
                SeedRooms();
            }
        }

        private void SeedRooms()
        {
            Console.WriteLine("Seeding rooms");
            var booking = new Booking
            {
                Id = "1",
                Email = "test@email.com",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today + TimeSpan.FromDays(2),
                NumberOfPeople = 2
            };
            var bookingList = new List<Booking> { booking };
            var roomCollection = new List<Room>
            {
                new Room
                {
                    Name = "First Room",
                    Description = "The first of many",
                    Address = "123 First Street",
                    Capacity = 4,
                    Price = 50.00,
                    Images = new List<string> { "https://picsum.photos/200", "https://picsum.photos/200" },
                    Bookings = bookingList
                },
                new Room
                {
                    Name = "Second Room",
                    Description = "The second of some more",
                    Address = "321 Second Ave",
                    Capacity = 4,
                    Price = 50.00,
                    Images = new List<string> { "https://picsum.photos/200" },
                    Bookings = bookingList
                }
            };
            Rooms.InsertMany(roomCollection);
        }
    }
}