using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
namespace ReactApp1.Server.Services;
public interface IReservationService
{
    Task<PaginatedResult<Reservation>> GetAllReservations(int pageSize, int pageNumber);
    Task<ReservationModel?> GetReservationById(int reservationId);
    Task<Reservation> CreateNewReservation(ReservationModel reservation, int? createdByEmployeeId);
    Task UpdateReservation(ReservationModel reservation);
    Task DeleteReservation(int reservationId);
}