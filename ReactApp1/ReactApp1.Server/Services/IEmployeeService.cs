using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IEmployeeService
{
    Task<PaginatedResult<Employee>> GetAllEmployees(int pageSize, int pageNumber);
    Task<EmployeeModel?> GetEmployeeById(int employeeId);
    Task CreateNewEmployee(EmployeeModel employee, int? establishmentId);
    Task UpdateEmployee(EmployeeModel employee);
    Task DeleteEmployee(int employeeId);
}