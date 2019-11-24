using System;
using System.Threading.Tasks;
using CQRS_Assignment.Commands;
using CQRS_Assignment.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CQRS_Assignment.Tests
{
    public class CommandsTest
    {
        [Fact]
        public async Task test_AddEmployee()
        {
            //Assemble

            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: "in_memory_db")
                .Options;

            Employee employee;
            using (var context = new EmployeeContext(options))
            {
                var commands = new CommandService(context);

                //act
                var result = await commands.AddEmployee(new Employee(){Department = "dept1", Name = "user1"});
            }

            using (var context = new EmployeeContext(options))
            {
                //Assert
                Assert.True(await context.Employees.AnyAsync(p => p.Department == "dept1"));
            }
        }

        [Fact]
        public async Task test_UpdateEmployee()
        {
            //Assemble

            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: "in_memory_db")
                .Options;

            Employee employee;
            using (var context = new EmployeeContext(options))
            {
                var commands = new CommandService(context);

                //act
                var addResult = await commands.AddEmployee(new Employee() { Department = "dept1", Name = "user1" });
                var updateResult = await commands.UpdateEmployee(new Employee() { Id = 1, Department = "dept2", Name = "user1" });
            }

            using (var context = new EmployeeContext(options))
            {
                //Assert
                Assert.True(await context.Employees.AnyAsync(p => p.Department == "dept2"));
            }
        }
    }
}
