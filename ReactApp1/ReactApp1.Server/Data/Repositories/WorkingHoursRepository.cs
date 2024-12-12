using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class WorkingHoursRepository : IWorkingHoursRepository
    {
        private readonly AppDbContext _context;

        public WorkingHoursRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<WorkingHours>> GetAllWorkingHoursAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Set<WorkingHours>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var workingHourss = await _context.Set<WorkingHours>()
                .OrderBy(workingHours => workingHours.WorkingHoursId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<WorkingHours>
            {
                Items = workingHourss,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }

        public async Task<WorkingHoursModel?> GetWorkingHoursByIdAsync(int workingHoursId)
        {
            var workingHours = await _context.WorkingHours
                .Where(f => f.WorkingHoursId == workingHoursId)
                .Select(f => new WorkingHoursModel()
                {
                    WorkingHoursId = f.WorkingHoursId,
                    EstablishmentAddressId = f.EstablishmentAddressId,
                    DayOfWeek = (DayOfWeekEnum)f.DayOfWeek,
                    StartTime = f.StartTime,
                    EndTime = f.EndTime,
                    ReceiveTime = f.ReceiveTime,
                    CreatedByEmployeeId = f.CreatedByEmployeeId
                }).FirstOrDefaultAsync();

            return workingHours;
        }

        public async Task<WorkingHours> AddWorkingHoursAsync(WorkingHoursModel workingHours)
        {
            try
            {
                var newWorkingHours = new WorkingHours
                {
                    EstablishmentAddressId = workingHours.EstablishmentAddressId,
                    DayOfWeek = (int)workingHours.DayOfWeek,
                    StartTime = workingHours.StartTime,
                    EndTime = workingHours.EndTime,
                    ReceiveTime = workingHours.ReceiveTime,
                    CreatedByEmployeeId = workingHours.CreatedByEmployeeId
                };

                var workingHoursEntity = await _context.WorkingHours.AddAsync(newWorkingHours);
                await _context.SaveChangesAsync();

                return workingHoursEntity.Entity;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new working hours to the database.", e);
            }
        }

        public async Task UpdateWorkingHoursAsync(WorkingHoursModel workingHours)
        {
            try
            {
                var existingWorkingHours = await _context.Set<WorkingHours>()
                    .FirstOrDefaultAsync(i => i.WorkingHoursId == workingHours.WorkingHoursId);


                if (existingWorkingHours == null)
                {
                    throw new KeyNotFoundException($"Working Hours with ID {workingHours.WorkingHoursId} not found.");
                }

                workingHours.MapUpdate(existingWorkingHours);
                _context.Set<WorkingHours>().Update(existingWorkingHours);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the working hours: {workingHours.WorkingHoursId}.", e);
            }
        }

        public async Task DeleteWorkingHoursAsync(int workingHoursId)
        {
            try
            {
                _context.Set<WorkingHours>().Remove(new WorkingHours
                {
                    WorkingHoursId = workingHoursId
                });

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the working hours {workingHoursId} from the database.", e);
            }
        }
    }
}

