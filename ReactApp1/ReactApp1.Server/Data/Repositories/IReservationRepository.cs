using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
namespace ReactApp1.Server.Data.Repositories
{
    public interface IReservationRepository
    {
        Task<PaginatedResult<Reservation>> GetAllReservationsAsync(int pageNumber, int pageSize, IPrincipal user);
        Task<ReservationModel?> GetReservationByIdAsync(int reservationId, IPrincipal user);
        Task<Reservation> AddReservationAsync(ReservationModel reservation);
        Task UpdateReservationAsync(ReservationModel reservation);
        Task DeleteReservationAsync(int reservationId, IPrincipal user);
    }
}