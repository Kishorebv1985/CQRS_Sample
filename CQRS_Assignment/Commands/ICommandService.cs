using System.Threading.Tasks;
using CQRS_Assignment.Model;

namespace CQRS_Assignment.Commands
{
    public interface ICommandService
    {
        Task<bool> AddEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
    }
}
