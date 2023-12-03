using System.Text.Json;

var mongoClient = new MongoClient("your_mongodb_connectionstring");
var database = mongoClient.GetDatabase("your_mongodb_database_name");
var collection = database.GetCollection<Flight>("your_mongo_db_collection_name");


BsonDocumentCommand<BsonDocument> command = new BsonDocumentCommand<BsonDocument>(
	BsonDocument.Parse(@"
                        { createIndexes: 'Flights', 
                          indexes: [{ 
                            name: 'vectorSearchIndex', 
                            key: { embedding: 'cosmosSearch' }, 
                            cosmosSearchOptions: { kind: 'vector-ivf', numLists: 5, similarity: 'COS', dimensions: 1536 } 
                          }] 
					}"
));


BsonDocument result = _mongoDatabase.RunCommand(command);
if (result["ok"] != 1)
{
	Console.WriteLine("CreateIndex failed with response: " + result.ToJson());
}