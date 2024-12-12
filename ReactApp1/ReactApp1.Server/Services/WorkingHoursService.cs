using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class WorkingHoursService : IWorkingHoursService
    {
        private readonly IWorkingHoursRepository _workingHoursRepository;

        public WorkingHoursService(IWorkingHoursRepository serviceRepository)
        {
            _workingHoursRepository = serviceRepository;
        }

        public Task<PaginatedResult<WorkingHours>> GetAllWorkingHours(int pageSize, int pageNumber)
        {
            return _workingHoursRepository.GetAllWorkingHoursAsync(pageSize, pageNumber);
        }

        public Task<WorkingHoursModel?> GetWorkingHoursById(int workingHoursId)
        {
            return _workingHoursRepository.GetWorkingHoursByIdAsync(workingHoursId);
        }

        public Task<WorkingHours> CreateNewWorkingHours(WorkingHoursModel workingHours)
        {
            return _workingHoursRepository.AddWorkingHoursAsync(workingHours);
        }

        public Task UpdateWorkingHours(WorkingHoursModel workingHours)
        {
            return _workingHoursRepository.UpdateWorkingHoursAsync(workingHours);
        }

        public Task DeleteWorkingHours(int workingHoursId)
        {
            return _workingHoursRepository.DeleteWorkingHoursAsync(workingHoursId);
        }
    }
}