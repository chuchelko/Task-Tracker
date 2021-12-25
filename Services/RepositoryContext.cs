using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;

namespace TaskTracker.Services
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public RepositoryContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaskDB;Trusted_Connection=True;");
        }
    }
}