using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AiTrip.Domain.Entities;
using AiTrip.Domain.Interfaces;
using AiTrip.Infrastructure.Configurations;

namespace AiTrip.Infrastructure.Database
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly ISecretVault _secretVault;


        public ProductRepository(IOptions<DatabaseConfiguration> databaseConfiguration, ISecretVault secretVault)
        {
            var configuration = databaseConfiguration.Value;

            _secretVault = secretVault;
            var connectionString = _secretVault.GetSecret(configuration.ConnectionStringSecretName);

           
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(configuration.DatabaseName);
            _productCollection = database.GetCollection<Product>(configuration.CollectionName);

        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Product?> GetAsync(int id)
        {
            return await _productCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
        }


    }
}
