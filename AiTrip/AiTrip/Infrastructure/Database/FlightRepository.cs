﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AiTrip.Domain.Entities;
using AiTrip.Domain.Interfaces;
using AiTrip.Infrastructure.Configurations;

namespace AiTrip.Infrastructure.Database
{
    public class FlightRepository : IRepository<Flight>
    {
        private readonly IMongoCollection<Flight> _flightCollection;
        private readonly ISecretVault _secretVault;


        public FlightRepository(IOptions<DatabaseConfiguration> databaseConfiguration, ISecretVault secretVault)
        {
            var configuration = databaseConfiguration.Value;

            _secretVault = secretVault;
            var connectionString = _secretVault.GetSecret(configuration.ConnectionStringSecretName);

           
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(configuration.DatabaseName);
            _flightCollection = database.GetCollection<Flight>(configuration.CollectionName);

        }

        public async Task<List<Flight>> GetAsync()
        {
            return await _flightCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Flight?> GetAsync(int id)
        {
            return await _flightCollection.Find(x => x.FlightId == id).FirstOrDefaultAsync();
        }


    }
}