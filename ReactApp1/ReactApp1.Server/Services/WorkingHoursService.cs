using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class WorkingHoursService : IWorkingHoursService
    {
        private readonly IWorkingHoursRepository _workingHoursRepository;
        private readonly Logger<WorkingHoursService> _logger;

        public WorkingHoursService(IWorkingHoursRepository serviceRepository, Logger<WorkingHoursService> logger)
        {
            _workingHoursRepository = serviceRepository;
            _logger = logger;
        }

        public Task<PaginatedResult<WorkingHours>> GetAllWorkingHours(int pageSize, int pageNumber)
        {
            return _workingHoursRepository.GetAllWorkingHoursAsync(pageSize, pageNumber);
        }

        public Task<WorkingHoursModel?> GetWorkingHoursById(int workingHoursId)
        {
            return _workingHoursRepository.GetWorkingHoursByIdAsync(workingHoursId);
        }

        public Task<WorkingHours> CreateNewWorkingHours(WorkingHoursModel workingHours, int? createdByEmployeeId)
        {
            if (!createdByEmployeeId.HasValue)
            {
                _logger.LogError("Failed to create working hours: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            return _workingHoursRepository.AddWorkingHoursAsync(workingHours, createdByEmployeeId.Value);
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