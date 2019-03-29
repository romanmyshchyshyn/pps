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

        public TmDbContext(DbContextOptions<TmDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(e => e.IsTeamLead).WithOne(e => e.Teamlead)
                .HasForeignKey<Team>(e => e.TeamLeadId);
        }
    }
}
