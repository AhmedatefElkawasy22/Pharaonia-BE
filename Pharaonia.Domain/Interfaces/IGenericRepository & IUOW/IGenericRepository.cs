
using System.Linq.Expressions;

namespace Pharaonia.Domain.Interfaces.IGenericRepository___IUOW
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(List<Expression<Func<T, object>>> includes = null, Expression<Func<T, bool>> match = null);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> match ,List<Expression<Func<T, object>>> includes = null);
        Task<T?> FirstAsync(Expression<Func<T, bool>> match = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
