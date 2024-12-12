using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;
namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WorkingHoursController : ControllerBase
    {
        private readonly IWorkingHoursService _workingHoursService;

        public WorkingHoursController(IWorkingHoursService workingHoursService)
        {
            _workingHoursService = workingHoursService;
        }

        #region Endpoints

        /// <summary>
        /// Get all working hours
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("workingHours")]
        public async Task<IActionResult> GetWorkingHourss(int pageNumber, int pageSize)
        {
            var workingHours = await _workingHoursService.GetAllWorkingHours(pageNumber, pageSize);
            return Ok(workingHours);
        }

        /// <summary>
        /// Get working hours by id
        /// </summary>
        /// <param name="workingHoursId"></param>
        /// <returns></returns>
        [HttpGet("workingHours/{workingHoursId}")]
        public async Task<IActionResult> GetWorkingHourss(int workingHoursId)
        {
            var workingHours = await _workingHoursService.GetWorkingHoursById(workingHoursId);
            return Ok(workingHours);
        }

        /// <summary>
        /// Create new working hours
        /// </summary>
        /// <param name="workingHours"></param>
        /// <returns></returns>
        [HttpPost("workingHours")]
        public async Task<IActionResult> CreateWorkingHours([FromBody] WorkingHoursModel workingHours)
        {
            var createdByEmployeeId = User.GetUserId();
            var newWorkingHours = await _workingHoursService.CreateNewWorkingHours(workingHours, createdByEmployeeId);

            return Ok(newWorkingHours);
        }

        /// <summary>
        /// Update existing working hours
        /// </summary>
        /// <param name="workingHours"></param>
        /// <returns></returns>
        [HttpPut("workingHours/{workingHoursId}")]
        public async Task<IActionResult> UpdateWorkingHours([FromBody] WorkingHoursModel workingHours)
        {
            await _workingHoursService.UpdateWorkingHours(workingHours);

            return Ok();
        }

        /// <summary>
        /// Delete working hours
        /// </summary>
        /// <param name="workingHoursId"></param>
        /// <returns></returns>
        [HttpDelete("workingHourss/{workingHoursId}")]
        public async Task<IActionResult> DeleteWorkingHours(int workingHoursId)
        {
            await _workingHoursService.DeleteWorkingHours(workingHoursId);

            return NoContent();
        }
        #endregion Endpoints
    }
}