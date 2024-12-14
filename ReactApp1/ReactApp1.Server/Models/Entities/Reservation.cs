using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Reservation")]
public class Reservation
{
    [Key]
    [Column("ReservationId")]
    public int ReservationId { get; set; }

    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }

    [Column("StartTime")]
    public DateTime? StartTime { get; set; }

    [Column("EndTime")]
    public DateTime? EndTime { get; set; }

    [Column("CreatedByEmployeeId")]
    public int CreatedByEmployeeId { get; set; }

    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }

    [Column("EstablishmentAddressId")]
    public int EstablishmentAddressId { get; set; }

    [Column("ServiceId")]
    public int ServiceId { get; set; }

    [Column("CustomerPhoneNumber")]
    public string CustomerPhoneNumber { get; set; }

    [ForeignKey(nameof(CreatedByEmployeeId))]
    public virtual Employee CreatedByEmployee { get; set; }

    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment Establishment { get; set; }

    [ForeignKey(nameof(EstablishmentAddressId))]
    public virtual EstablishmentAddress EstablishmentAddress { get; set; }

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; }

    public virtual Order Order { get; set; }
}