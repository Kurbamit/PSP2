using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IEstablishmentService
{
    Task<PaginatedResult<Establishment>> GetAllEstablishments(int pageSize, int pageNumber);
    Task<EstablishmentModel?> GetEstablishmentById(int establishmentId);
    Task CreateNewEstablishment(EstablishmentModel establishment, int? establishmentId);
    Task UpdateEstablishment(EstablishmentModel establishment);
    Task DeleteEstablishment(int establishmentId);
}