using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InternshipDistribution.Repositories
{
    public class CompanyRepository : BaseRepository<Company>
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Company>> GetAllAsync(Expression<Func<Company, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .Where(c => c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }
    }
}
