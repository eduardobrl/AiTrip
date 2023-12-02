namespace AiTrip.Domain.Entities
{
    public class ChatCompletion
    {
	    public string Response { get; set; } = string.Empty;
        public int PromptTokens { get; set; }
        public int ResponseTokens { get; set; }
    }
}
