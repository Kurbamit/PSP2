using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.Models.Models.Domain
{
    public class EstablishmentModel
    {
        /// <summary>
        /// Identifikatorius
        /// </summary>
        public int EstablishmentId { get; set; }

        /// <summary>
        /// Adreso identifikatorius
        /// </summary>
        public int EstablishmentAddressId { get; set; }

        /// <summary>
        /// Tipas
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Valstybe
        /// </summary>
        [StringLength(255)]
        public string? Country { get; set; }

        /// <summary>
        /// Miestas
        /// </summary>
        [StringLength(255)]
        public string? City { get; set; }

        /// <summary>
        /// Gatve
        /// </summary>
        [StringLength(255)]
        public string? Street { get; set; }

        /// <summary>
        /// Gatves numeris
        /// </summary>
        [StringLength(255)]
        public string? StreetNumber { get; set; }

        public void MapUpdate(Establishment existingEstablishment)
        {
            existingEstablishment.Type = this.Type;

            if (existingEstablishment.EstablishmentAddress != null)
            {
                existingEstablishment.EstablishmentAddress.Country = this.Country;
                existingEstablishment.EstablishmentAddress.City = this.City;
                existingEstablishment.EstablishmentAddress.Street = this.Street;
                existingEstablishment.EstablishmentAddress.StreetNumber = this.StreetNumber;
            }
        }
    }
}
