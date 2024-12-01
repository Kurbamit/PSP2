using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentService _establishmentService;

        public EstablishmentController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        #region Endpoints

        /// <summary>
        /// Get all establishments
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("establishments")]
        public async Task<IActionResult> GetEstablishments(int pageNumber, int pageSize)
        {
            var establishments = await _establishmentService.GetAllEstablishments(pageNumber, pageSize);

            return Ok(establishments);
        }

        /// <summary>
        /// Get establishment by id
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <returns></returns>
        [HttpGet("establishments/{establishmentId}")]
        public async Task<IActionResult> GetEstablishments(int establishmentId)
        {
            var establishment = await _establishmentService.GetEstablishmentById(establishmentId);

            return Ok(establishment);
        }

        /// <summary>
        /// Create new establishment
        /// </summary>
        /// <param name="establishment"></param>
        /// <returns></returns>
        [HttpPost("establishments")]
        public async Task<IActionResult> CreateEstablishment([FromBody] EstablishmentModel establishment)
        {
            var establishmentId = User.GetUserEstablishmentId();
            await _establishmentService.CreateNewEstablishment(establishment, establishmentId);

            return Ok(establishment.EstablishmentId);
        }

        /// <summary>
        /// Update existing establishment
        /// </summary>
        /// <param name="establishment"></param>
        /// <returns></returns>
        [HttpPut("establishments/{establishmentId}")]
        public async Task<IActionResult> UpdateEstablishment([FromBody] EstablishmentModel establishment)
        {
            await _establishmentService.UpdateEstablishment(establishment);

            return Ok();
        }

        /// <summary>
        /// Delete establishment
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <returns></returns>
        [HttpDelete("establishments/{establishmentId}")]
        public async Task<IActionResult> DeleteEstablishment(int establishmentId)
        {
            await _establishmentService.DeleteEstablishment(establishmentId);

            return Ok();
        }

        #endregion Endpoints
    }
}