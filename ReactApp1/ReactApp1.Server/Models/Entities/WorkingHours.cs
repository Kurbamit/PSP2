using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("WorkingHours")]
public class WorkingHours
{
    [Key]
    [Column("WorkingHoursId")]
    public int WorkingHoursId { get; set; }

    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }

    [Column("EstablishmentId")]
    public int EstablishmentAddressId { get; set; }

    [Column("DayOfWeek")]
    public int DayOfWeek { get; set; }

    [Column("StartTime")]
    public TimeSpan StartTime { get; set; }

    [Column("EndTime")]
    public TimeSpan EndTime { get; set; }

    [Column("CreatedByEmployeeId")]
    public int CreatedByEmployeeId { get; set; }

    [ForeignKey(nameof(EstablishmentAddressId))]
    public virtual EstablishmentAddress EstablishmentAddress { get; set; }

    [ForeignKey(nameof(CreatedByEmployeeId))]
    public virtual Employee CreatedByEmployee { get; set; }
}