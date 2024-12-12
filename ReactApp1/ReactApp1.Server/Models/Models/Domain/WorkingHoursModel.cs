using System.ComponentModel.DataAnnotations;
using ReactApp1.Server.Models.Enums;

namespace ReactApp1.Server.Models.Models.Domain
{
    public class WorkingHoursModel
    {
        /// <summary>
        /// Identifikatorius
        /// </summary>
        public int WorkingHoursId { get; set; }

        /// <summary>
        /// Imones adreso identifikatorius
        /// </summary>
        public int EstablishmentAddressId { get; set; }

        /// <summary>
        /// Savaites diena
        /// </summary>
        public DayOfWeekEnum DayOfWeek { get; set; }

        /// <summary>
        /// Trukme
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Sukurimo/modifikavimo laikas
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// Identifikatorius darbuotojo, kuris sukure darbo valandas
        /// </summary>
        public int CreatedByEmployeeId { get; set; }

        public void MapUpdate(WorkingHours existingWorkingHours)
        {
            existingWorkingHours.EstablishmentAddressId = this.EstablishmentAddressId;
            existingWorkingHours.DayOfWeek = (int)this.DayOfWeek;
            existingWorkingHours.StartTime = this.StartTime;
            existingWorkingHours.EndTime = this.EndTime;
            existingWorkingHours.CreatedByEmployeeId = this.CreatedByEmployeeId;
        }
    }
}