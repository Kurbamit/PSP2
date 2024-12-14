using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("FullOrder")]
public class FullOrder
{
    public FullOrder()
    {
        Items = new HashSet<Item>();
    }
    
    [Key]
    public int FullOrderId { get; set; }

    [Column("OrderId")]
    public int OrderId { get; set; }
    
    [Column("ItemId")]
    public int ItemId { get; set; }
    
    [Column("Count")]
    public int Count { get; set; }
    
    [Column("Name")]
    public string? Name { get; set; }
    
    [Column("Cost")]
    public decimal? Cost { get; set; }
    
    [Column("Tax")]
    public decimal? Tax { get; set; }
    
    [Column("AlcoholicBeverage")]
    public bool AlcoholicBeverage { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("CreatedByEmployeeId")]
    public int? CreatedByEmployeeId { get; set; }
    
    // Navigation property for Order
    public virtual Order Order { get; set; }
    
    [ForeignKey(nameof(CreatedByEmployeeId))]
    public virtual Employee CreatedByEmployee { get; set; }

    // Navigation property for Item
    public virtual ICollection<Item> Items { get; set; }
}