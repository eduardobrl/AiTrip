using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AiTrip.Domain.Entities;
using AiTrip.Domain.Interfaces;
using AiTrip.Infrastructure.Configurations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace AiTrip.Infrastructure.Database
{
    public class FlightRepository : IRepository<Flight>
    {
        private readonly IMongoCollection<BsonDocument> _bsonCollection;
        private readonly IMongoCollection<Flight> _flightCollection;
        private readonly int _maxVectorSearchResults = 3;


		public FlightRepository(IOptions<DatabaseConfiguration> databaseConfiguration, ISecretVault secretVault)
        {
	        var configuration = databaseConfiguration.Value;

	        var connectionString = secretVault.GetSecret(configuration.ConnectionStringSecretName);

           
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(configuration.DatabaseName);
            _flightCollection = database.GetCollection<Flight>(configuration.CollectionName);
            _bsonCollection = database.GetCollection<BsonDocument>(configuration.CollectionName);

		}

        public async Task<List<Flight>> GetAsync()
        {
			return await _flightCollection.Find(_ => true, options:new FindOptions
			{
				
			}).ToListAsync();
		}


        public async Task<List<Flight>> VectorSearchAsync(Embedding embedding)
        {
	        var queryVector = embedding.Value ?? new float[]{};
	        List<string> retDocs = new List<string>();

	        string resultDocuments = string.Empty;

	        try
	        {
		        var formattedQueryVector = queryVector.Select(x => x.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
		        var json1 =
			        $"{{$search: {{cosmosSearch: {{ vector: [{string.Join(',', formattedQueryVector)}], path: 'embedding', k: {_maxVectorSearchResults}}}, returnStoredSource:true}}}}";
		        var json2 = $"{{$project: {{embedding: 0}}}}";

				BsonDocument[] pipeline = new BsonDocument[]
		        {
			        BsonDocument.Parse(json1),
			        BsonDocument.Parse(json2),
		        };

				var bsonDocuments = await _flightCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

		        var flights = bsonDocuments.ToList().ConvertAll(bsonDocument => BsonSerializer.Deserialize<Flight>(bsonDocument));
		        return flights;
	        }
	        catch (MongoException ex)
	        {
		        Console.WriteLine($"Exception: VectorSearchAsync(): {ex.Message}");
		        throw;
	        }

        }

        public Task AddAsync(Flight entity, Embedding? embedding = default)
        {
	        if (embedding == null)
	        {
				return Task.CompletedTask;
	        }
	        var document = entity.ToBsonDocument();
	        document.Add("embedding", new BsonArray(embedding.Value));

			_bsonCollection.InsertOne(document);

			return Task.CompletedTask;
        }


        public async Task<Flight?> GetAsync(string id)
        {
            return await _flightCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }


    }
}
