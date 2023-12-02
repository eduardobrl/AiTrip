using System.Globalization;

namespace AiTrip.Domain.Formatters
{
	public static class CurrencyFormatter
	{
		public const string Dollar = "$";
		public static string ToString(decimal currency)
		{
			return Dollar + currency.ToString(CultureInfo.InvariantCulture);
		}
	}
}
