using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Item")]
public class Item
{
    public Item()
    {
        FullOrders = new HashSet<FullOrder>();
    }
    
    [Key]
    [Column("ItemId")]
    [Required] public int ItemId { get; set; }
    
    [Column("Name")]
    [StringLength(255)]
    public string? Name { get; set; }
    
    [Column("Cost")]
    public decimal? Cost { get; set; }
    
    [Column("Tax")]
    public decimal? Tax { get; set; }
    
    [Column("AlcoholicBeverage")]
    public bool AlcoholicBeverage { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    // Navigation property to storage. One-to-One relationship
    public virtual Storage? Storage { get; set; }
    
    public virtual ICollection<FullOrder> FullOrders { get; set; }
}