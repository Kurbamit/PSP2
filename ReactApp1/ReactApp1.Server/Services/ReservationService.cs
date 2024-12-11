using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
namespace ReactApp1.Server.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger<OrderService> _logger;

        public ReservationService(IReservationRepository reservationRepository, ILogger<OrderService> logger)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public Task<PaginatedResult<Reservation>> GetAllReservations(int pageSize, int pageNumber)
        {
            return _reservationRepository.GetAllReservationsAsync(pageSize, pageNumber);
        }

        public Task<ReservationModel?> GetReservationById(int reservationId)
        {
            return _reservationRepository.GetReservationByIdAsync(reservationId);
        }

        public Task<Reservation> CreateNewReservation(ReservationModel reservation, int? createdByEmployeeId)
        {
            if (!createdByEmployeeId.HasValue)
            {
                _logger.LogError("Failed to open order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            reservation.CreatedByEmployeeId = createdByEmployeeId.Value;

            return _reservationRepository.AddReservationAsync(reservation);
        }

        public Task UpdateReservation(ReservationModel reservation)
        {
            return _reservationRepository.UpdateReservationAsync(reservation);
        }

        public Task DeleteReservation(int reservationId)
        {
            return _reservationRepository.DeleteReservationAsync(reservationId);
        }
    }
}
