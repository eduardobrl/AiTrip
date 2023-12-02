using AiTrip.Domain.Entities;

namespace AiTrip.Domain.Interfaces
{
	public interface IOpenAiService
	{
		Task<Embedding?> GetEmbeddingsAsync(params string[] data);

		Task<ChatCompletion> GetChatCompletionAsync(
			string userPrompt, string documents);

		Task<ChatCompletion> GetDestinationCompletionAsync(
			string destination);

	}
}
