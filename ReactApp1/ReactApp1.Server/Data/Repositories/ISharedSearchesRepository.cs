using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Data.Repositories;

public interface ISharedSearchesRepository
{
    public Task<List<SharedItem>> GetAllItems(int establishmentId, string? search);
}