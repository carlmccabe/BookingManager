using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingManager.API.Models;

public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string? Id { get; set; }
    public string? RoomId { get; set; }
    public string? Email { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfPeople { get; set; }
}