using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS_Assignment.Commands;
using CQRS_Assignment.Model;
using CQRS_Assignment.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IQueriesService _queries;
        private readonly ICommandService _commands;

        public EmployeesController(IQueriesService queries, ICommandService commands)
        {
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
        {
            return (await _queries.GetAllEmployees()).Select(e => new EmployeeDto()
            {
                Id = e.Id,
                Name = e.Name,
                Department = e.Department
            }).ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetById(int id)
        {
            return (await _queries.GetAllEmployees()).Select(e => new EmployeeDto()
            {
                Id = e.Id,
                Name = e.Name,
                Department = e.Department
            }).ToList();
        }


        [HttpPost]
        public void AddEmployee([FromBody] EmployeeDto employee)
        {
            _commands.AddEmployee(new Employee(){Name = employee.Name, Department = employee.Department});
        }

        [HttpPut]
        public void UpdateEmployee([FromBody] EmployeeDto employee)
        {
            _commands.UpdateEmployee(new Employee() {Id = employee.Id, Name = employee.Name, Department = employee.Department });
        }

    }
}
