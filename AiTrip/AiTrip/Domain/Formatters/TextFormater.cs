namespace AiTrip.Domain.Formatters
{
	public static class TextFormater
	{
		public static string[] GetParagraphs(string longtext)
		{
			return longtext.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
