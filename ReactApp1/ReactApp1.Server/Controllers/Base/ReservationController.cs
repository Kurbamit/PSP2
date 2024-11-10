using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Data.Repositories;
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
        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations(int pageNumber, int pageSize)
        {
            var reservations = await _reservationService.GetAllReservations(pageNumber, pageSize);
            return Ok(reservations);
        }

        [HttpGet("reservations/{reservationId}")]
        public async Task<IActionResult> GetReservations(int reservationId)
        {
            var reservation = await _reservationService.GetReservationById(reservationId);
            return Ok(reservation);
        }

        [HttpPost("reservations")]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {

            await _reservationService.CreateNewReservation(reservation);

            return Ok(reservation.ReservationId);
        }

        [HttpPut("reservations/{reservationId}")]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationModel reservation)
        {
            await _reservationService.UpdateReservation(reservation);

            return Ok();
        }

        [HttpDelete("reservations/{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            await _reservationService.DeleteReservation(reservationId);

            return NoContent();
        }
        #endregion Endpoints
    }
}