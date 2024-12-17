using System.ComponentModel.DataAnnotations;
using ReactApp1.Server.Models.Enums;

namespace ReactApp1.Server.Models.Models.Domain
{
    public class ServiceModel
    {
        /// <summary>
        /// Identifikatorius
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// Serviso pavadinimas
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Imones identifikatorius
        /// </summary>
        public int EstablishmentId { get; set; }

        /// <summary>
        /// Trukme
        /// </summary>
        public TimeSpan ServiceLength { get; set; }

        /// <summary>
        /// Kaina
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// Sukurimo/modifikavimo laikas
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        public int? Count { get; set; }
        public decimal? Discount { get; set; }
        public string? DiscountName { get; set; }
        public List<TaxModel>? Taxes { get; set; }

        public void MapUpdate(Service existingModel)
        {
            existingModel.Name = Name;
            existingModel.EstablishmentId = this.EstablishmentId;
            existingModel.ServiceLength = this.ServiceLength;
            existingModel.Cost = this.Cost;
        }
    }
}