using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Options;
using AiTrip.Domain.Interfaces;
using AiTrip.Infrastructure.Configurations;

namespace AiTrip.Infrastructure.Secrets
{
    public class AzureKeyVault : ISecretVault
    {
        private readonly VaultConfiguration _configuration;
        private readonly SecretClient _client;

        public AzureKeyVault(IOptions<VaultConfiguration> configuration)
        {
            _configuration = configuration.Value;
            var credential = new DefaultAzureCredential();

            var keyVaultUri = new Uri($"https://{_configuration.KeyVaultName}.vault.azure.net/");

            _client = new SecretClient(keyVaultUri, credential);
        }
        public async Task<string?> GetSecretAsync(string key)
        {
            var envSecret = Environment.GetEnvironmentVariable(key);
            if (envSecret != null)
            {
                return envSecret;
            }

            var secret = await _client.GetSecretAsync(key);

            if (!secret.HasValue)
            {
                return default;
            }

            return secret.Value.Value;
        }

        public string? GetSecret(string key)
        {
            var envSecret = Environment.GetEnvironmentVariable(key);
            if (envSecret != null)
            {
                return envSecret;
            }

            var secret = _client.GetSecret(key);

            if (!secret.HasValue)
            {
                return default;
            }

            return secret.Value.Value;
        }
    }

}
