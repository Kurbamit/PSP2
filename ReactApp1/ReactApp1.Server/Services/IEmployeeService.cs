using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IEmployeeService
{
    Task<PaginatedResult<Employee>> GetAllEmployees(int pageSize, int pageNumber, IPrincipal user);
    Task<EmployeeModel?> GetEmployeeById(int employeeId, IPrincipal user);
    Task<int> CreateNewEmployee(EmployeeModel employee, int? establishmentId);
    Task UpdateEmployee(EmployeeModel employee);
    Task DeleteEmployee(int employeeId);
}