using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Service")]
public class Service
{
    [Key]
    [Column("ServiceId")]
    public int ServiceId { get; set; }

    [Column("Name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }

    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }

    [Column("ServiceLength")]
    public TimeSpan ServiceLength { get; set; }

    [Column("Cost")]
    public decimal? Cost { get; set; }

    [Column("Tax")]
    public decimal? Tax { get; set; }

    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment Establishment { get; set; }
}