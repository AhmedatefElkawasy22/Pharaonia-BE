
namespace Pharaonia.Infrastructure.GenericRepository___UOW
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(List<Expression<Func<T, object>>> includes = null, Expression<Func<T, bool>> match = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            if (match != null)
                query = query.Where(match);

            return await query.ToListAsync();
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> match , List<Expression<Func<T, object>>> includes = null)
        {
            if (match != null)
            {
                IQueryable<T> query = _context.Set<T>();

                if (includes != null)
                    foreach (var include in includes)
                        query = query.Include(include);

                    return await query.SingleOrDefaultAsync(match);
            }
            return null;
        }
        public async Task<T?> FirstAsync(Expression<Func<T, bool>> match = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(match != null)
                query = query.Where(match);
            return await query.FirstOrDefaultAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

    }
}
