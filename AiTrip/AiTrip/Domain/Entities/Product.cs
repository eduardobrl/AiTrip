using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AiTrip.Domain.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    public int ProductId { get; set; } = 0;
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string ProductCategory { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;

}