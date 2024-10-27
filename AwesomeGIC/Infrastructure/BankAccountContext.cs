using AwesomeGIC.Domain;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGIC.Infrastructure
{
    public class BankAccountContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<InterestRule> InterestRules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-JSDRQCEO;Database=GICBank;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InterestRule>()
               .Property(b => b.Rate)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<BankAccount>()
               .Property(b => b.Balance)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
               .Property(b => b.Amount)
               .HasColumnType("decimal(18,2)");
        }
    }
}
