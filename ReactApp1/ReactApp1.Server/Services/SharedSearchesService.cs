using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Services
{
    public class SharedSearchesService : ISharedSearchesService
    {
        private readonly ISharedSearchesRepository _sharedSearchesRepository;
        
        public SharedSearchesService(ISharedSearchesRepository sharedSearchesRepository)
        {
            _sharedSearchesRepository = sharedSearchesRepository;
        }
        
        public async Task<List<SharedItem>> GetAllItems(int establishmentId, string? search)
        {
            return await _sharedSearchesRepository.GetAllItems(establishmentId, search);
        }
    }
}