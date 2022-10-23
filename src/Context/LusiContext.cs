using LuSiBack.src.models;
using Microsoft.EntityFrameworkCore;

namespace LuSiBack.src.Context
{
    public class LusiContext : DbContext
    {
        public LusiContext(DbContextOptions<LusiContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskOption>()
                .HasOne<TaskUser>(s => s.TaskUser)
                .WithMany(g => g.TaskOptions)
                .HasForeignKey(s => s.CurrentTaskId);

            modelBuilder.Entity<TaskUser>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.TaskUsers)
                .HasForeignKey(s => s.UserTaskId);

            
        }

        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<TaskOption> TaskOptions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}