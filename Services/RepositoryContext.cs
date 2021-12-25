using Microsoft.EntityFrameworkCore;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Services
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