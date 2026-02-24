using ClosureServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext>options) : base(options)
        {

        }
        public DbSet<Foreclosure> Foreclosures { get; set; }
        public DbSet<LoanClosure> LoanClosures { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Foreclosure>().HasKey(x => x.ForeclosureId);
            modelBuilder.Entity<LoanClosure>().HasKey(x => x.ClosureId);

            modelBuilder.Entity<Foreclosure>().ToTable("Foreclosures");
            modelBuilder.Entity<LoanClosure>().ToTable("LoanClosures");


            modelBuilder.Entity<Foreclosure>().Property(x => x.ApprovalStatus).HasConversion<string>();
            modelBuilder.Entity<Foreclosure>().Property(x => x.ForeclosureType).HasConversion<string>();
            modelBuilder.Entity<Foreclosure>().Property(x => x.PaymentMode).HasConversion<string>();


            modelBuilder.Entity<LoanClosure>().Property(x => x.ClosureType).HasConversion<string>();
            modelBuilder.Entity<LoanClosure>().Property(x => x.ClosureStatus).HasConversion<string>();








            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.PrincipalOutstanding)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.InterestOutstanding)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.PenaltyOutstanding)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.ForeclosureCharges)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.RebateAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.TotalPayable)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Foreclosure>()
                .Property(x => x.SavingsAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.TotalDisbursed)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.TotalPrincipalPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.TotalInterestPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.TotalPenaltyPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.TotalAmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.OutstandingAtClosure)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.ClosureCharges)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.RebateAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.WriteOffAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanClosure>()
                .Property(x => x.SettlementAmount)
                .HasPrecision(18, 2);
        }
    }
}
