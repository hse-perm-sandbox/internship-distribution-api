using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipDistribution.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(n => n.Email == email && n.DeletedAt == null);
        }
    }
}
