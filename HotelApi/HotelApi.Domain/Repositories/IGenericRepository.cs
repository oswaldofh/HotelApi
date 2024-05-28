using System.Linq.Expressions;

namespace HotelApi.Domain.Repositories
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int? id);
        Task<T> Insert(T entity);
        Task<T> Delete(int id);
        Task Update(T entity);
        Task<T> Find(Expression<Func<T, bool>> expr);
    }
}
