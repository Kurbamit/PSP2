using Microsoft.AspNetCore.Authorization;
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
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        #region Endpoints

        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations(int pageNumber, int pageSize)
        {
            var reservations = await _reservationService.GetAllReservations(pageNumber, pageSize, User);
            return Ok(reservations);
        }

        /// <summary>
        /// Get reservation by id
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("reservations/{reservationId}")]
        public async Task<IActionResult> GetReservations(int reservationId)
        {
            var reservation = await _reservationService.GetReservationById(reservationId, User);
            return Ok(reservation);
        }

        /// <summary>
        /// Create new reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("reservations")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationModel reservation)
        {
            var userId = User.GetUserId();
            var newReservation = await _reservationService.CreateNewReservation(reservation, userId);

            return Ok(newReservation);
        }

        /// <summary>
        /// Update existing reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("reservations/{reservationId}")]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationModel reservation)
        {
            await _reservationService.UpdateReservation(reservation);

            return Ok();
        }

        /// <summary>
        /// Delete reservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("reservations/{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            await _reservationService.DeleteReservation(reservationId, User);

            return NoContent();
        }
        #endregion Endpoints
    }
}