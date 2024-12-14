using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        
        #region Endpoints
        
        /// <summary>
        /// Get all employees
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees(int pageNumber, int pageSize)
        {
            var employees = await _employeeService.GetAllEmployees(pageNumber, pageSize);

            return Ok(employees);
        }
        
        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("employees/{employeeId}")]
        public async Task<IActionResult> GetEmployees(int employeeId)
        {
            var employee = await _employeeService.GetEmployeeById(employeeId);

            return Ok(employee);
        }

        /// <summary>
        /// Get the currently logged in employee
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("employees/current")]
        public async Task<IActionResult> GetCurrentEmployee()
        {
            var userId = User.GetUserId();
            if (userId.HasValue)
            {
                var employee = await _employeeService.GetEmployeeById(userId.Value);
                return Ok(employee);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeModel employee)
        {
            var establishmentId = User.GetUserEstablishmentId();
            var employeeId = await _employeeService.CreateNewEmployee(employee, establishmentId);
            
            return Ok(employeeId);
        }
        
        /// <summary>
        /// Update existing employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("employees/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeModel employee)
        {
            await _employeeService.UpdateEmployee(employee);
            
            return Ok();
        }
        
        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            await _employeeService.DeleteEmployee(employeeId);
            
            return NoContent();
        }
        
        #endregion Endpoints
    }
}