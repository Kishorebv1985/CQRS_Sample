using System;
using System.Threading.Tasks;
using CQRS_Assignment.Model;

namespace CQRS_Assignment.Commands
{
    public class CommandService : ICommandService
    {
        private readonly EmployeeContext _context;

        public CommandService(EmployeeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddEmployee(Employee employee)
        {
            try
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
