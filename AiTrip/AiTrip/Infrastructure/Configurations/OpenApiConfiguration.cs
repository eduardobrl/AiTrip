namespace AiTrip.Infrastructure.Configurations
{
	public class OpenApiConfiguration
	{

		public const string Section = "OpenAi";
		public string ProxyUrl { get; set; } = string.Empty;
        public string TokenSecretName { get; set; } = string.Empty;
		public string EmbeddingDeployment { get; set; } = string.Empty;
		public string ChatCompletionDeployment { get; set; } = string.Empty;
		public int MaxTokens { get; set; } = 2048;
	}
}
