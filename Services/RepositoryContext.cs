using Microsoft.EntityFrameworkCore;
using System;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Repositories
{
    public class RepositoryContext : DbContext
    {
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public RepositoryContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaskDB;Trusted_Connection=True;");
        }
    }
}