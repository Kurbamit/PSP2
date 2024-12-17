using System.Security.Principal;
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

        public async Task<List<SharedItem>> GetAllItems(int? establishmentId, string? search, IPrincipal user)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all items: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            return await _sharedSearchesRepository.GetAllItems(establishmentId.Value, search, user);
        }

        public async Task<List<SharedService>> GetAllServices(int? establishmentId, string? search, IPrincipal user)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all services: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            return await _sharedSearchesRepository.GetAllServices(establishmentId.Value, search, user);
        }
        
        public async Task<List<SharedItem>> GetAllDiscounts(int? establishmentId, string? search, IPrincipal user)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all items: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            return await _sharedSearchesRepository.GetAllDiscounts(establishmentId.Value, search, user);
        }

        public async Task<List<SharedItem>> GetAllTaxes(string? search)
        {
            return await _sharedSearchesRepository.GetAllTaxes(search);
        }
        public async Task<List<SharedItem>> GetAllBaseItemsForEdit(int? establishmentId, string? search)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all items: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            return await _sharedSearchesRepository.GetAllBaseItemsForEdit(establishmentId.Value, search);
        }
        public async Task<List<SharedItem>> GetAllBaseItems(int? establishmentId, string? search)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all items: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            return await _sharedSearchesRepository.GetAllBaseItems(establishmentId.Value, search);
        }
        public async Task<List<SharedItem>> GetAllItemsVariations(int? establishmentId, string? search, int itemId)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to fetch all items: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            return await _sharedSearchesRepository.GetAllItemsVariations(establishmentId.Value, search, itemId);
        }
    }
}