using Microsoft.EntityFrameworkCore;
using Scrum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrum.DataAccess
{
    public class ScrumContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketList> TicketLists { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User_Project> User_Projects { get; set; }

        public ScrumContext(DbContextOptions<ScrumContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<Project>().HasKey(x => x.Id);
            modelBuilder.Entity<Project>().HasMany(x => x.TicketLists).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<TicketList>().HasKey(x => x.Id);
            modelBuilder.Entity<TicketList>().HasMany(x => x.Tickets).WithOne(x => x.TicketList).HasForeignKey(x => x.TaskListId);

            modelBuilder.Entity<Ticket>().HasKey(x => x.Id);

            modelBuilder.Entity<User_Project>().HasKey(x => new { x.UserId, x.ProjectId });
            modelBuilder.Entity<User_Project>().HasOne(x => x.User).WithMany(x => x.User_Projects).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User_Project>().HasOne(x => x.Project).WithMany(x => x.User_Projects).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
