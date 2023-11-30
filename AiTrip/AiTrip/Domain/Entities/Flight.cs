using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AiTrip.Domain.Entities;

public class Flight
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int FlightId { get; set; } = 0;
    public string FlightName { get; set; } = string.Empty;
    public string FlightDescription { get; set; } = string.Empty;
    public string FlightImage { get; set; } = string.Empty;

}