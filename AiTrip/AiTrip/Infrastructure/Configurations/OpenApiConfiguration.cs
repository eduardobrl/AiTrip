namespace AiTrip.Infrastructure.Configurations
{
	public class OpenApiConfiguration
	{

		public const string Section = "OpenAi";
		public string ProxyUrl { get; set; }
        public string TokenSecretName { get; set; }
		public string EmbeddingDeployment { get; set; }
	}
}
