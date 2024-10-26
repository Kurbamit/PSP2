using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("FullOrder")]
public class FullOrder
{
    [Key]
    public int FullOrderId { get; set; }

    [Column("OrderId")]
    public int OrderId { get; set; }
    
    [Column("ItemId")]
    public int ItemId { get; set; }
    
    [Column("Count")]
    public int Count { get; set; }
    
    // Navigation property for Order
    public virtual Order Order { get; set; }

    // Navigation property for Item
    public virtual Item Item { get; set; }
}