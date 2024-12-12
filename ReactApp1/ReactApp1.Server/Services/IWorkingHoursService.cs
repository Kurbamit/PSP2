using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IWorkingHoursService
{
    Task<PaginatedResult<WorkingHours>> GetAllWorkingHours(int pageSize, int pageNumber);
    Task<WorkingHoursModel?> GetWorkingHoursById(int workingHoursId);
    Task<WorkingHours> CreateNewWorkingHours(WorkingHoursModel workingHours);
    Task UpdateWorkingHours(WorkingHoursModel workingHours);
    Task DeleteWorkingHours(int workingHoursId);
}