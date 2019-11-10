using CashDesk.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashDesk
{
    public class CashDeskDataContext : DbContext
    {
        public DbSet<Member> Member { get; set; }

        public DbSet<Membership> Membership { get; set; }

        public DbSet<Deposit> Deposit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("CashDesk");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Memberships)
                .WithOne(ms => ms.Member)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Membership>()
                .HasMany(ms => ms.Deposits)
                .WithOne(d => d.Membership)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
