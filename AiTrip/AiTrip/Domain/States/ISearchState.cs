using System.Reactive.Subjects;
using AiTrip.Domain.Interfaces;

namespace AiTrip.Domain.States
{
	public class SearchState : ISearchService
	{
		private Subject<string> _search = new();

		public void Search(string search)
		{
			_search.OnNext(search);
		}

		public ISubject<string> GetSearch()
		{
			return _search;
		}
	}
}
