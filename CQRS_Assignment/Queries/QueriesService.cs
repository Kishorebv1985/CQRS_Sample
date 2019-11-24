using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CQRS_Assignment.Controllers;
using CQRS_Assignment.Model;
using Dapper;

namespace CQRS_Assignment.Queries
{
    public class QueriesService : IQueriesService
    {
        private readonly string _connectionString;

        public QueriesService(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("message", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return await conn.QueryAsync<Employee>("SELECT Id, Name, Department FROM dbo.Employees;");
            }
        }
        
        public async Task<Employee> GetEmployeeById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var results = await conn.QueryAsync<Employee>($"SELECT Id, Name, Department FROM dbo.Employees where Id ={id};");
                return results.FirstOrDefault();
            }
        }
    }
}
