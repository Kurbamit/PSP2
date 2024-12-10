using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<PaginatedResult<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Set<Employee>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            var employees = await _context.Set<Employee>()
                .OrderBy(employee => employee.EmployeeId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<Employee>
            {
                Items = employees,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }

        public async Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _context.Employees
                .Where(f => f.EmployeeId == employeeId)
                .Select(f => new EmployeeModel()
                {
                    EmployeeId = f.EmployeeId,
                    EstablishmentId = f.EstablishmentId,
                    Title = (TitleEnum)f.Title,
                    PersonalCode = f.PersonalCode,
                    FirstName = f.FirstName,
                    LastName = f.LastName,
                    BirthDate = f.BirthDate,
                    Phone = f.Phone,
                    Email = f.Email,
                    Country = f.EmployeeAddress.Country,
                    City = f.EmployeeAddress.City,
                    Street = f.EmployeeAddress.Street,
                    StreetNumber = f.EmployeeAddress.StreetNumber,
                    HouseNumber = f.EmployeeAddress.HouseNumber,
                    ReceiveTime = f.ReceiveTime
                }).FirstOrDefaultAsync();
            
            return employee;
        }
        
        public async Task<int> AddEmployeeAsync(EmployeeModel employee, int? establishmentId)
        {
            if (!establishmentId.HasValue)
            {
                throw new ArgumentNullException(nameof(establishmentId));
            }
            try
            {
                var passwordHash = GeneratePasswordHash(employee.PasswordHash);
                
                using var transaction = await _context.Database.BeginTransactionAsync();
                var newEmployee = new Employee
                {
                    Title = (int)employee.Title,
                    PersonalCode = employee.PersonalCode,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    BirthDate = employee.BirthDate,
                    Phone = employee.Phone,
                    Email = employee.Email,
                    PasswordHash = passwordHash,
                    EstablishmentId = establishmentId.Value
                };
                await _context.Employees.AddAsync(newEmployee);
                await _context.SaveChangesAsync();

                var address = new EmployeeAddress
                {
                    Country = employee.Country,
                    City = employee.City,
                    Street = employee.Street,
                    StreetNumber = employee.StreetNumber,
                    HouseNumber = employee.HouseNumber,
                    EmployeeId = newEmployee.EmployeeId
                };
                
                await _context.EmployeeAddresses.AddAsync(address);
                await _context.SaveChangesAsync();
                
                newEmployee.AddressId = address.AddressId;
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
                
                return newEmployee.EmployeeId;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new employee to the database.", e);
            }
        }
        
        public async Task UpdateEmployeeAsync(EmployeeModel employee)
        {
            try
            {
                var existingEmployee = await _context.Set<Employee>()
                    .Include(e => e.EmployeeAddress)
                    .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
                
                if (existingEmployee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {employee.EmployeeId} not found.");
                }

                if (!string.IsNullOrWhiteSpace(employee.PasswordHash))
                {
                    existingEmployee.PasswordHash = GeneratePasswordHash(employee.PasswordHash);
                }
                
                employee.MapUpdate(existingEmployee);
                
                _context.Set<Employee>().Update(existingEmployee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the employee: {employee.EmployeeId}.", e);
            }
        }
        
        public async Task DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                _context.Set<Employee>().Remove(new Employee
                {
                    EmployeeId = employeeId 
                });
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the employee {employeeId} from the database.", e);
            }
        }
        
        private string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

