using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipDistribution.Repositories
{
    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> SaveResumeNameAsync(int id, string fileName)
        {
            var student = await GetByIdAsync(id);

            if (student != null)
            {
                student.Resume = fileName;
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteResumeNameAsync(Student student)
        {
            student.Resume = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
