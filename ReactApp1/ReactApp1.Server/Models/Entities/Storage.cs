using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Storage")]
public class Storage
{
    [Key]
    [Column("StorageId")]
    public int StorageId { get; set; }
    
    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }
    
    [Column("ItemId")]
    public int ItemId { get; set; }
    
    [Column("Count")]
    public int Count { get; set; }
    
    [ForeignKey(nameof(ItemId))]
    public virtual Item? Item { get; set; }
    
    // Foreign key to Establishment
    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment? Establishment { get; set; }
}