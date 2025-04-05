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

        private async Task SoftDeleteEntityAsync(BaseEntity entity, HashSet<BaseEntity> processed)
        {
            if (processed.Contains(entity))
                return;

            processed.Add(entity);

            if (entity.DeletedAt == null)
                entity.DeletedAt = DateTime.UtcNow;

            var entry = _context.Entry(entity);

            foreach (var navigation in entry.Navigations)
            {
                if (!navigation.IsLoaded)
                    await navigation.LoadAsync();

                if (navigation.CurrentValue is IEnumerable<BaseEntity> collection)
                {
                    foreach (var dependent in collection)
                        if (dependent.DeletedAt == null)
                            await SoftDeleteEntityAsync(dependent, processed);
                }

                else if (navigation.CurrentValue is BaseEntity dependentEntity)
                {
                    if (dependentEntity.DeletedAt == null)
                        await SoftDeleteEntityAsync(dependentEntity, processed);
                }
            }
        }

        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null || entity.DeletedAt != null)
                return false;

            var processed = new HashSet<BaseEntity>();

            await SoftDeleteEntityAsync(entity, processed);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
