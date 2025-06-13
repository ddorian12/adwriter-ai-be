using AdWriter_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<GeneratedItem> GeneratedItems { get; set; }
        public DbSet<GenerationPack> GenerationPacks { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.GeneratedItems)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // GeneratedItem
            modelBuilder.Entity<GeneratedItem>()
                .Property(g => g.ResultJson)
                .HasColumnType("nvarchar(max)");

            // GenerationPack
            modelBuilder.Entity<GenerationPack>()
                .Property(p => p.PriceRon)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<GenerationPack>()
                .HasMany(p => p.Payments)
                .WithOne(p => p.Pack)
                .HasForeignKey(p => p.GenerationPackId);
        }
    }

}
