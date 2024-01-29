
using FormulaApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FormulaApi.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DriversDbContext _context;
        internal DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(DriversDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await _dbSet.ToListAsync();
        }
        public virtual async Task<T?> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }
        public virtual async Task<bool> Update(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }
        public virtual async Task<bool> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }
        
    }
}
