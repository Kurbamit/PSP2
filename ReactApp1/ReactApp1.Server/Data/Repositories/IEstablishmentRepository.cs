using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IEstablishmentRepository
    {
        Task<PaginatedResult<Establishment>> GetAllEstablishmentsAsync(int pageNumber, int pageSize);
        Task<EstablishmentModel?> GetEstablishmentByIdAsync(int establishmentId);
        Task AddEstablishmentAsync(EstablishmentModel establishment, int? establishmentId);
        Task UpdateEstablishmentAsync(EstablishmentModel establishment);
        Task DeleteEstablishmentAsync(int establishmentId);
    }
}