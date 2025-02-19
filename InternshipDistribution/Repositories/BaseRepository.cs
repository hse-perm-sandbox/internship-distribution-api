using InternshipDistribution.Models;

namespace InternshipDistribution.Repositories
{
    public class BaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
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
