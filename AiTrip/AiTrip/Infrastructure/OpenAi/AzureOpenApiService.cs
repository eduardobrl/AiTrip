using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using AiTrip.Domain.Interfaces;
using AiTrip.Infrastructure.Configurations;

namespace AiTrip.Infrastructure.OpenAi
{
	public class AzureOpenApiService
	{
        private readonly OpenAIClient _client;
        private readonly OpenApiConfiguration _configuration;
        private readonly ISecretVault _secretVault;
        private const string ProxyBasePath = "/v1/api";

        public AzureOpenApiService(IOptions<OpenApiConfiguration> configuration, ISecretVault secretVault)
        {
            _configuration = configuration.Value;
            _secretVault = secretVault;

            var tokenText = _secretVault.GetSecret(_configuration.TokenSecretName);
            if (tokenText == null)
            {
                throw new ArgumentException();
            }

            Uri proxyUri = new(_configuration.ProxyUrl + ProxyBasePath);
            AzureKeyCredential token = new(tokenText);
            _client = new OpenAIClient(proxyUri, token);
            
        }

        public async Task<float[]?> GetEmbeddingsAsync(dynamic data)
        {
            try
            {
                EmbeddingsOptions options = new ()
                {
                    Input = data,
                    DeploymentName = _configuration.EmbeddingDeployment
                };

                var response = await _client.GetEmbeddingsAsync(options);

                Embeddings embeddings = response.Value;

                float[] embedding = embeddings.Data[0].Embedding.ToArray();

                return embedding;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetEmbeddingsAsync Exception: {ex.Message}");
                return null;
            }
        }


    }
}
