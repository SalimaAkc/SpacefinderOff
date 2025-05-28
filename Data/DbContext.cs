using Microsoft.EntityFrameworkCore;
using SpacefinderOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SpacefinderOff.Data
{
    public class SpacefinderContext : DbContext
    {
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }

        public SpacefinderContext(DbContextOptions<SpacefinderContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID);

            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.Classroom)
                .WithMany()
                .HasForeignKey(b => b.ClassroomID);
        }
    }
}
