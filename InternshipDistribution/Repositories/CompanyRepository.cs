using InternshipDistribution.Enums;
using InternshipDistribution.Models;
using InternshipDistribution.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InternshipDistribution.Repositories
{
    public class CompanyRepository : BaseRepository<Company>
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

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

        public override async Task<bool> SoftDeleteAsync(int companyId)
        {
            await base.SoftDeleteAsync(companyId);

            var applications = await _context.DistributionApplications
                .Where(app => app.DeletedAt == null &&
                             (app.Priority1CompanyId == companyId ||
                              app.Priority2CompanyId == companyId ||
                              app.Priority3CompanyId == companyId))
                .ToListAsync();

            foreach (var app in applications)
            {
                ClearPriority(app, companyId, 1);
                ClearPriority(app, companyId, 2);
                ClearPriority(app, companyId, 3);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private void ClearPriority(DistributionApplication app, int companyId, int priorityNumber)
        {
            switch (priorityNumber)
            {
                case 1:
                    if (app.Priority1CompanyId == companyId)
                    {
                        app.Priority1CompanyId = null;
                        app.Priority1Status = PriorityStatus.NotSelected;
                    }
                    break;
                case 2:
                    if (app.Priority2CompanyId == companyId)
                    {
                        app.Priority2CompanyId = null;
                        app.Priority2Status = PriorityStatus.NotSelected;
                    }
                    break;
                case 3:
                    if (app.Priority3CompanyId == companyId)
                    {
                        app.Priority3CompanyId = null;
                        app.Priority3Status = PriorityStatus.NotSelected;
                    }
                    break;
            }
        }
    }
}
