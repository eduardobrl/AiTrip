using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AiTrip.Domain.Entities;

[BsonIgnoreExtraElements]
public class Flight
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string FlightId { get; set; } = string.Empty;
    public string FlightDescription { get; set; } = string.Empty;
    public string FlightImage { get; set; } = string.Empty;
    public string FlightOrigin { get; set; } = string.Empty;
    public string FlightDestiny { get; set; } = string.Empty;
    public decimal FlightPrice { get; set; } = 0;
    public decimal FlightCurrentPrice { get; set; } = 0;

}