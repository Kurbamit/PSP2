using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IEstablishmentRepository _establishmentRepository;

        public EstablishmentService(IEstablishmentRepository establishmentRepository)
        {
            _establishmentRepository = establishmentRepository;
        }

        public Task<PaginatedResult<Establishment>> GetAllEstablishments(int pageSize, int pageNumber)
        {
            return _establishmentRepository.GetAllEstablishmentsAsync(pageSize, pageNumber);
        }

        public Task<EstablishmentModel?> GetEstablishmentById(int establishmentId)
        {
            return _establishmentRepository.GetEstablishmentByIdAsync(establishmentId);
        }

        public Task CreateNewEstablishment(EstablishmentModel establishment, int? establishmentId)
        {
            return _establishmentRepository.AddEstablishmentAsync(establishment, establishmentId);
        }

        public Task UpdateEstablishment(EstablishmentModel establishment)
        {
            return _establishmentRepository.UpdateEstablishmentAsync(establishment);
        }

        public Task DeleteEstablishment(int establishmentId)
        {
            return _establishmentRepository.DeleteEstablishmentAsync(establishmentId);
        }
    }
}