using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTracker.Benchmarks
{
    internal class PersonDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "Persons");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(_ => _.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Persons { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}