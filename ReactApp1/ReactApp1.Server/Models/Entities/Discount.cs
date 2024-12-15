using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Discount")]
public class Discount
{
    public Discount()
    {
        Orders = new HashSet<Order>();
        Items = new HashSet<FullOrder>();
    }
    
    [Key]
    [Column("DiscountId")]
    public int DiscountId { get; set; }
    
    [Column("Name")]
    public string Name { get; set; }
    
    [Column("Percentage")]
    public decimal Percentage { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }
    
    [Column("ValidFrom")]
    public DateTime? ValidFrom { get; set; }
    
    [Column("ValidTo")]
    public DateTime? ValidTo { get; set; }
    
    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment Establishment { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
    
    // If individual items can have discounts
    public virtual ICollection<FullOrder> Items { get; set; }
}