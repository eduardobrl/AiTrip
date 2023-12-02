using AiTrip.Domain.Entities;

namespace AiTrip.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAsync();
        Task<T?> GetAsync(string id);
        Task<List<Flight>> VectorSearchAsync(Embedding queryVector);
        Task AddAsync(T entity, Embedding? embedding = default);

	}
}
