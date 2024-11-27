using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<PaginatedResult<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeId);
        Task AddEmployeeAsync(EmployeeModel employee, int? establishmentId);
        Task UpdateEmployeeAsync(EmployeeModel employee);
        Task DeleteEmployeeAsync(int employeeId);
    }
}