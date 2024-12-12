using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IWorkingHoursRepository
    {
        Task<PaginatedResult<WorkingHours>> GetAllWorkingHoursAsync(int pageNumber, int pageSize);
        Task<WorkingHoursModel?> GetWorkingHoursByIdAsync(int establishmentAddressId);
        Task<WorkingHours> AddWorkingHoursAsync(WorkingHoursModel workingHours, int createdByEmployeeId);
        Task UpdateWorkingHoursAsync(WorkingHoursModel workingHours);
        Task DeleteWorkingHoursAsync(int workingHoursId);
    }
}