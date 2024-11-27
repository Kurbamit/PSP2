using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<PaginatedResult<Employee>> GetAllEmployees(int pageSize, int pageNumber)
        {
            return _employeeRepository.GetAllEmployeesAsync(pageSize, pageNumber);
        }

        public Task<EmployeeModel?> GetEmployeeById(int employeeId)
        {
            return _employeeRepository.GetEmployeeByIdAsync(employeeId);
        }

        public Task CreateNewEmployee(EmployeeModel employee, int? establishmentId)
        {
            return _employeeRepository.AddEmployeeAsync(employee, establishmentId);
        }

        public Task UpdateEmployee(EmployeeModel employee)
        {
            return _employeeRepository.UpdateEmployeeAsync(employee);
        }

        public Task DeleteEmployee(int employeeId)
        {
            return _employeeRepository.DeleteEmployeeAsync(employeeId);
        }
    }
}