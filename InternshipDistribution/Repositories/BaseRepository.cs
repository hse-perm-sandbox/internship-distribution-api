using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipDistribution.Repositories
{
    public class BaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => e.DeletedAt == null).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity?.DeletedAt == null ? entity : null;
        }
        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null || entity.DeletedAt != null)
                return false;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null || entity.DeletedAt !=  null)
            {
                return false;
            }

            entity.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
