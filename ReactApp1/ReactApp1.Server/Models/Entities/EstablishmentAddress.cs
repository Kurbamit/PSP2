using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("EstablishmentAddress")]
public class EstablishmentAddress
{
    [Key]
    [Column("AddressId")]
    public int AddressId { get; set; }

    [Column("Country")]
    [StringLength(255)]
    public string? Country { get; set; }

    [Column("City")]
    [StringLength(255)]
    public string? City { get; set; }
    
    [Column("Street")]
    [StringLength(255)]
    public string? Street { get; set; }
    
    [Column("StreetNumber")]
    [StringLength(255)]
    public string? StreetNumber { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }

    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }
    
    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment? Establishment { get; set; }
}