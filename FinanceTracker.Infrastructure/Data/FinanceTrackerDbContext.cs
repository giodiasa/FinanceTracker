using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Data
{
    public class FinanceTrackerDbContext : DbContext
    {
        public FinanceTrackerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>()
                .Property(x => x.Name)
                .HasMaxLength(50);
            builder.Entity<Category>()
                .HasMany(x => x.Transactions)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
            builder.Entity<Transaction>()
                .HasOne(x => x.User)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.UserId);
        }
    }
}
