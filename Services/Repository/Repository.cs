using AuthenAutherApp.Data.AppDbContext;
using AuthenAutherApp.Services.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AuthenAutherApp.Services.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task AddAsync(T entity)
        {
            _dbSet.AddAsync(entity);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }
            _dbSet.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id) ?? throw new ArgumentException("Entity not found");
        }

        public  Task UpdateAsync(T entity)
        {
           _dbSet.Update(entity);
           return _context.SaveChangesAsync();
        }
    }
}