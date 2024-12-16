using System.Security.Principal;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Task<PaginatedResult<Employee>> GetAllEmployees(int pageSize, int pageNumber, IPrincipal user)
        {
            return _employeeRepository.GetAllEmployeesAsync(pageSize, pageNumber, user);
        }

        public Task<EmployeeModel?> GetEmployeeById(int employeeId, IPrincipal user)
        {
            return _employeeRepository.GetEmployeeByIdAsync(employeeId, user);
        }

        public Task<int> CreateNewEmployee(EmployeeModel employee, int? establishmentId)
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