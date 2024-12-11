using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;
namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        #region Endpoints

        /// <summary>
        /// Get all services
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("services")]
        public async Task<IActionResult> GetServices(int pageNumber, int pageSize)
        {
            var services = await _serviceService.GetAllServices(pageNumber, pageSize);
            return Ok(services);
        }

        /// <summary>
        /// Get service by id
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpGet("services/{serviceId}")]
        public async Task<IActionResult> GetServices(int serviceId)
        {
            var service = await _serviceService.GetServiceById(serviceId);
            return Ok(service);
        }

        /// <summary>
        /// Create new service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost("services")]
        public async Task<IActionResult> CreateService([FromBody] ServiceModel service)
        {
            var userId = User.GetUserId();
            var newService = await _serviceService.CreateNewService(service, userId);

            return Ok(newService);
        }

        /// <summary>
        /// Update existing service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPut("services/{serviceId}")]
        public async Task<IActionResult> UpdateService([FromBody] ServiceModel service)
        {
            await _serviceService.UpdateService(service);

            return Ok();
        }

        /// <summary>
        /// Delete service
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            await _serviceService.DeleteService(serviceId);

            return NoContent();
        }
        #endregion Endpoints
    }
}