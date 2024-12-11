﻿using System.ComponentModel.DataAnnotations;
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
        /// Mokesčiai
        /// </summary>
        public decimal? Tax { get; set; }

        /// <summary>
        /// Sukurimo/modifikavimo laikas
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        public void MapUpdate(Service existingModel)
        {
            existingModel.EstablishmentId = this.EstablishmentId;
            existingModel.ServiceLength = this.ServiceLength;
            existingModel.Cost = this.Cost;
            existingModel.Tax = this.Tax;
        }
    }
}