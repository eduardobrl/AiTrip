using System.Reactive.Subjects;

namespace AiTrip.Domain.Interfaces
{
	public interface ISearchService
	{
		void Search(string search);
		ISubject<string> GetSearch();
		ISubject<bool> SearchFinished();
	}
}
