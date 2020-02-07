using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using testPumox.Domain;

namespace testPumox.Persistence
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options) { }

        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(BuildCompany);
            modelBuilder.Entity<Employee>(BuildEmployee);
        }

        private void BuildCompany(EntityTypeBuilder<Company> entityTypeBuilder)
        {
            entityTypeBuilder.Property(c => c.Name).IsRequired();
            entityTypeBuilder.Property(c => c.EstablishmentYear).IsRequired();

            entityTypeBuilder
                .HasMany(c => c.Employees)
                .WithOne(e => e.Company)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void BuildEmployee(EntityTypeBuilder<Employee> entityTypeBuilder)
        {
            entityTypeBuilder.Property(e => e.FirstName).IsRequired();
            entityTypeBuilder.Property(e => e.LastName).IsRequired();
            entityTypeBuilder.Property(e => e.DateOfBirth).IsRequired();
            entityTypeBuilder.Property(e => e.JobTitle).IsRequired();

            entityTypeBuilder
                .HasOne(employer => employer.Company)
                .WithMany(company => company.Employees);
        }
    }
}