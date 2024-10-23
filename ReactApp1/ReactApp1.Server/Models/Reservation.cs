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
    
    [Column("CustomerCount")]
    public int? CustomerCount { get; set; }
    
    [Column("ReservedSpot")]
    [StringLength(255)]
    public string? ReservedSpot { get; set; }
    
    public virtual Order Order { get; set; }
}