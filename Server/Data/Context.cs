using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Role> Roles { get; set; }
        // public DbSet<Proffesion> Proffesions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Discussion> Discussions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().ToTable("Appointment")
                .HasOne(e => e.Client)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>().ToTable("Appointment")
                .HasOne(e => e.Doctor)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Diagnosis>().ToTable("Diagnosis");
            modelBuilder.Entity<Gender>().ToTable("Gender");
            modelBuilder.Entity<Result>().ToTable("Result");
            modelBuilder.Entity<Role>().ToTable("Role");
            // modelBuilder.Entity<Proffesion>().ToTable("Proffesion");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Discussion>().ToTable("Discussion").HasOne(x => x.Doctor).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>().ToTable("Review").HasOne(x => x.Doctor).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>().ToTable("Review").HasOne(x => x.Client).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}