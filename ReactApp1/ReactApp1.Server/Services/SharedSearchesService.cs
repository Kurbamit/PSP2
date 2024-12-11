using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Services
{
    public class SharedSearchesService : ISharedSearchesService
    {
        private readonly ISharedSearchesRepository _sharedSearchesRepository;
        private readonly ILogger<SharedSearchesService> _logger;
        
        public SharedSearchesService(ISharedSearchesRepository sharedSearchesRepository, ILogger<SharedSearchesService> logger)
        {
            _sharedSearchesRepository = sharedSearchesRepository;
            _logger = logger;
        }

        public async Task<List<SharedItem>> GetAllItems(int? establishmentId, string? search)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all items: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            return await _sharedSearchesRepository.GetAllItems(establishmentId.Value, search);
        }
    }
}