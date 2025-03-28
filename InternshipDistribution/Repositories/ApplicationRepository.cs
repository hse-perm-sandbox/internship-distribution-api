using InternshipDistribution.Enums;
using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipDistribution.Repositories
{
    public class ApplicationRepository : BaseRepository<DistributionApplication>
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<DistributionApplication>> GetByStatusAsync(ApplicationStatus status)
        {
            return await _dbSet
                .Where(a => a.Status == status && a.DeletedAt == null)
                .ToListAsync();
        }

        public async Task UpdatePriorityStatusAsync(int applicationId, int priorityNumber, PriorityStatus status)
        {
            var application = await _dbSet.FindAsync(applicationId);
            if (application == null) return;

            switch (priorityNumber)
            {
                case 1: application.Priority1Status = status; break;
                case 2: application.Priority2Status = status; break;
                case 3: application.Priority3Status = status; break;
                default: throw new ArgumentException("Invalid priority number");
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DistributionApplication?> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet
                .Include(a => a.Student)
                .Include(a => a.Priority1Company)
                .Include(a => a.Priority2Company)
                .Include(a => a.Priority3Company)
                .Where(a =>
                    a.StudentId == studentId &&
                    a.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
    }
}
