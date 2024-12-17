using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
namespace ReactApp1.Server.Services;
public interface IReservationService
{
    Task<PaginatedResult<Reservation>> GetAllReservations(int pageSize, int pageNumbe, IPrincipal user);
    Task<ReservationModel?> GetReservationById(int reservationId, IPrincipal user);
    Task<Reservation> CreateNewReservation(ReservationModel reservation, int? createdByEmployeeId);
    Task UpdateReservation(ReservationModel reservation);
    Task DeleteReservation(int reservationId, IPrincipal user);
}