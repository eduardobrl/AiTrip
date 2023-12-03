namespace AiTrip.Infrastructure.Database
{
	public class Pagination<T>
	{
		public List<T> Items { get; set; }
		public int PageNumber { get; set; } = 0;
		public int PageSize { get; set; }
		public int PageCount { get; set; }
		public int TotalCount { get; set; }
		public int TotalPageCount { get; set;}

	}
}
