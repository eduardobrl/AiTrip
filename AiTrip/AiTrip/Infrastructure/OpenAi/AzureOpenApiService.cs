using System.Text.Json.Serialization;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using AiTrip.Domain.Interfaces;
using AiTrip.Infrastructure.Configurations;
using AiTrip.Domain.Entities;
using MongoDB.Bson.IO;

namespace AiTrip.Infrastructure.OpenAi
{
    public class AzureOpenApiService : IOpenAiService
	{
        private readonly OpenAIClient _client;
        private readonly OpenApiConfiguration _configuration;
        private readonly ISecretVault _secretVault;
        private const string ProxyBasePath = "/v1/api";

        private const string SystemBasePrompt = """
                                                
                                                You are an helpful assistant for AiTrip, a company that sells travels packages.
                                                You are designed to help users given tips about places to travel
                                    
                                                    Instructions:
                                                - Only answer questions based in the provides travels packages.
                                                - Give the bests suggestion based on your knowledge about the destination and the user needs
                                    
                                                    For example:
                                                - User said: 'I want to go to a place with beach'
                                                - You can use your knowledge about the available destinations and say that the user can go to 'Rio de Janeiro' because it has nice beach
                                                    Obs: This is only one example, you don't need to use this response.
                                                
                                                Available destinations:
                                                        
                                                """;

        private const string DestinationCompletionPrompt = """

                                                You are an helpful assistant for AiTrip, a company that sells travels packages.
                                                You are designed to help employees register new destination in the platform.
                                                
                                                When a user say a destination you provide a rich description of that destination to assist the creation of a new travel package.
                                                        
                                                """;



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
            AzureKeyCredential token = new(tokenText + "/eduardobrl");
            _client = new OpenAIClient(proxyUri, token);
		}

        public async Task<Embedding?> GetEmbeddingsAsync(params string[] data)
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

                return new Embedding
                {
                    Value = embedding
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetEmbeddingsAsync Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<ChatCompletion> GetChatCompletionAsync(
	        string userPrompt, string documents)
        {
	        ChatMessage systemMessage = new ChatMessage(ChatRole.System, SystemBasePrompt + documents);
	        ChatMessage userMessage = new ChatMessage(ChatRole.User, userPrompt);


	        ChatCompletionsOptions options = new()
	        {

		        Messages =
		        {
			        systemMessage,
			        userMessage
		        },
		        MaxTokens = _configuration.MaxTokens,
		        Temperature = 0.7f,
		        NucleusSamplingFactor = 0.95f,
		        FrequencyPenalty = 0,
		        PresencePenalty = 0,
                DeploymentName =_configuration.ChatCompletionDeployment
	        };

	        Azure.Response<ChatCompletions> completionsResponse = await _client.GetChatCompletionsAsync(options);

	        ChatCompletions completions = completionsResponse.Value;

	        return new ChatCompletion{
                PromptTokens = completions.Usage.PromptTokens,
                Response = completions.Choices[0].Message.Content,
                ResponseTokens = completions.Usage.CompletionTokens
	        };

		}

        public async Task<ChatCompletion> GetDestinationCompletionAsync(
	        string destination)
        {
	        ChatMessage systemMessage = new ChatMessage(ChatRole.System, DestinationCompletionPrompt);
	        ChatMessage userMessage = new ChatMessage(ChatRole.User, destination);


	        ChatCompletionsOptions options = new()
	        {

		        Messages =
		        {
			        systemMessage,
			        userMessage
		        },
		        MaxTokens = 512,
		        Temperature = 0.7f,
		        NucleusSamplingFactor = 0.95f,
		        FrequencyPenalty = 0,
		        PresencePenalty = 0,
		        DeploymentName = _configuration.ChatCompletionDeployment
	        };

	        Azure.Response<ChatCompletions> completionsResponse = await _client.GetChatCompletionsAsync(options);

	        ChatCompletions completions = completionsResponse.Value;

	        return new ChatCompletion
	        {
		        PromptTokens = completions.Usage.PromptTokens,
		        Response = completions.Choices[0].Message.Content,
		        ResponseTokens = completions.Usage.CompletionTokens
	        };

        }


	}
}
