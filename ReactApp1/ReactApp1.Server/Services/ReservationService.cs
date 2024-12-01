using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
namespace ReactApp1.Server.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationRepository _reservationRepository;

        public ReservationService(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public Task<PaginatedResult<Reservation>> GetAllReservations(int pageSize, int pageNumber)
        {
            return _reservationRepository.GetAllReservationsAsync(pageSize, pageNumber);
        }

        public Task<ReservationModel?> GetReservationById(int reservationId)
        {
            return _reservationRepository.GetReservationByIdAsync(reservationId);
        }

        public Task CreateNewReservation(Reservation reservation)
        {
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
