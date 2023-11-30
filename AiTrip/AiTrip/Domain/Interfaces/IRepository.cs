using AiTrip.Domain.Entities;

namespace AiTrip.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAsync();
        Task<T?> GetAsync(int id);
    }
}
