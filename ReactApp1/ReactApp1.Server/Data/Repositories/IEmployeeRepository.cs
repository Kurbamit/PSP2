using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<PaginatedResult<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize, IPrincipal user);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeId, IPrincipal user);
        Task<int> AddEmployeeAsync(EmployeeModel employee, int? establishmentId);
        Task UpdateEmployeeAsync(EmployeeModel employee);
        Task DeleteEmployeeAsync(int employeeId);
    }
}