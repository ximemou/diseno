using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data.DataAccess
{
    public class DomainContext:DbContext
    {
       

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }

      


        public DomainContext(DbContextOptions<DomainContext> options) : base(options)
        {

        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTeam>()
                        .HasKey(u => new { u.UserId,u.TeamId });

            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Sport>()
             .HasMany<Team>(g => g.Teams)
             .WithOne(s => s.Sport)
             .HasForeignKey(s => s.SportId)
             .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
