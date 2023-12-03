using AiTrip.Domain.Entities;
using AiTrip.Infrastructure.Database;

namespace AiTrip.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<Pagination<T>> GetAsync(int pageNumber = 1, int pageSize = 10);
        Task<T?> GetAsync(string id);
        Task<List<Flight>> VectorSearchAsync(Embedding queryVector);
        Task AddAsync(T entity, Embedding? embedding = default);

	}
}
