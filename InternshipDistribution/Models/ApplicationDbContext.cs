using Microsoft.EntityFrameworkCore;

namespace InternshipDistribution.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // Получаем все измененные или добавленные сущности, которые наследуются от BaseEntity
            var entries = ChangeTracker.Entries().Where(
                    e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        // При добавлении сущности устанавливаем CreatedAt
                        entity.CreatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        // При изменении сущности обновляем UpdatedAt
                        entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        // При удалении сущности устанавливаем DeletedAt и отменяем физическое удаление
                        entry.State = EntityState.Modified;
                        entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }

            // Сохраняем изменения в базе данных
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();
        }
    }
}
