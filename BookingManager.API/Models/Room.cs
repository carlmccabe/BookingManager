using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingManager.API.Models;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")] public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public int Capacity { get; set; }
    public double Price { get; set; }
    public List<string>? Images { get; set; }
    public List<Booking>? Bookings { get; set; }
}