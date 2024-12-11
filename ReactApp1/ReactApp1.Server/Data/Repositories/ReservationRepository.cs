using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ReactApp1.Server.Data.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Reservation>> GetAllReservationsAsync(int pageNumber, int pageSize)
        {
            var totalReservations = await _context.Set<Reservation>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalReservations / (double)pageSize);

            var reservations = await _context.Set<Reservation>()
                .OrderBy(reservation => reservation.ReservationId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<Reservation>
            {
                Items = reservations,
                TotalPages = totalPages,
                TotalItems = totalReservations,
                CurrentPage = pageNumber
            };
        }

        public async Task<ReservationModel?> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _context.Reservations
                .Where(f => f.ReservationId == reservationId)
                .Select(f => new ReservationModel()
                {
                    ReservationID = f.ReservationId,
                    ReceiveTime = f.ReceiveTime,
                    StartTime = f.StartTime.Value,
                    EndTime = f.EndTime.Value,
                    CreatedByEmployeeId = f.CreatedByEmployeeId,
                    CustomerPhoneNumber = f.CustomerPhoneNumber
                }).FirstOrDefaultAsync();
            return reservation;
        }

        public async Task<Reservation> AddReservationAsync(ReservationModel reservation)
        {
            try
            {
                var newReservation = new Reservation
                {
                    ReceiveTime = reservation.ReceiveTime,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    CreatedByEmployeeId = reservation.CreatedByEmployeeId,
                    CustomerPhoneNumber = reservation.CustomerPhoneNumber
                };

                var reservationEntity = await _context.Reservations.AddAsync(newReservation);
                await _context.SaveChangesAsync();

                return reservationEntity.Entity;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new reservation to the database.", e);
            }
        }

        public async Task UpdateReservationAsync(ReservationModel reservaton)
        {
            try
            {
                var existingReservation = await _context.Set<Reservation>()
                    .FirstOrDefaultAsync(i => i.ReservationId == reservaton.ReservationID);


                if (existingReservation == null)
                {
                    throw new KeyNotFoundException($"Reservation with ID {reservaton.ReservationID} not found.");
                }
                reservaton.MapUpdate(existingReservation);
                _context.Set<Reservation>().Update(existingReservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the reservation: {reservaton.ReservationID}.", e);
            }
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            try
            {
                _context.Set<Reservation>().Remove(new Reservation
                {
                    ReservationId = reservationId
                });

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the reservation {reservationId} from the database.", e);
            }
        }
    }
}