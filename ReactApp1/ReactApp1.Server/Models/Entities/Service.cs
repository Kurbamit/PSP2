using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Service")]
public class Service
{
    public Service()
    {
        ServiceTax = new HashSet<ServiceTax>();
    }

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

    [Column("AssignedEmployeeId")]
    public int AssignedEmployeeId { get; set; }

    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment Establishment { get; set; }

    [ForeignKey(nameof(AssignedEmployeeId))]
    public virtual Employee AssignedEmployee { get; set; }

    public virtual ICollection<ServiceTax> ServiceTax { get; set; }
}