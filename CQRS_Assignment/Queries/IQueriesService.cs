using CQRS_Assignment.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS_Assignment.Queries
{
    public interface IQueriesService
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
    }
}
