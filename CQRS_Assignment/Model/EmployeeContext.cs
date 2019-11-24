using Microsoft.EntityFrameworkCore;

namespace CQRS_Assignment.Model
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeContext(DbContextOptions options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasData(new Employee() { Id = 1, Name = "Kishore", Department = "Engineering" });
        }
    }
}
