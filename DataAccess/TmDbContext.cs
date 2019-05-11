using DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class TmDbContext : IdentityDbContext<User>
    {
        public DbSet<CustomTask> CustomTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<CustomTaskStatus> CustomTaskStatuses { get; set; }

        public TmDbContext(DbContextOptions<TmDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>()
                .HasOne(e => e.Teamlead).WithOne()
                .HasForeignKey<Team>(e => e.TeamLeadId);

            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.UserName);

            modelBuilder.Entity<CustomTaskStatus>()
                .HasKey(cts => cts.Name);
        }
    }
}
