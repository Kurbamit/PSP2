using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("FullOrderService")]
public class FullOrderService
{
    public FullOrderService()
    {
        Services = new HashSet<Service>();
    }
    
    [Key]
    public int FullOrderId { get; set; }

    [Column("OrderId")]
    public int OrderId { get; set; }
    
    [Column("ServiceId")]
    public int ServiceId { get; set; }
    
    [Column("Count")]
    public int Count { get; set; }

    [Column("ServiceLength")]
    public TimeSpan ServiceLength { get; set; }

    [Column("Name")]
    public string? Name { get; set; }
    
    [Column("Cost")]
    public decimal? Cost { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("CreatedByEmployeeId")]
    public int? CreatedByEmployeeId { get; set; }

    [Column("AssignedEmployeeId")]
    public int AssignedEmployeeId { get; set; }

    [Column("DiscountId")]
    public int? DiscountId { get; set; }

    // Navigation property for Order
    public virtual Order Order { get; set; }
    
    [ForeignKey(nameof(CreatedByEmployeeId))]
    public virtual Employee CreatedByEmployee { get; set; }

    [ForeignKey(nameof(AssignedEmployeeId))]
    public virtual Employee AssignedEmployee { get; set; }

    [ForeignKey(nameof(DiscountId))]
    public virtual Discount Discount { get; set; }

    // Navigation property for Item
    public virtual ICollection<Service> Services { get; set; }
}