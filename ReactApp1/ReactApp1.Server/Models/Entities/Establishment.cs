using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Establishment")]
public class Establishment
{
    public Establishment()
    {
        Storages = new HashSet<Storage>();
        Employees = new HashSet<Employee>();
        Orders = new HashSet<Order>();
        Items = new HashSet<Item>();
        Discounts = new HashSet<Discount>();
    }
    
    [Key]
    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }

    [Column("EstablishmentAddressId")]
    public int EstablishmentAddressId { get; set; }
    
    [Column("Type")]
    public int Type { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    // Navigation property for the related EstablishmentAddress
    public virtual EstablishmentAddress? EstablishmentAddress { get; set; }
    
    // Navigation property for the related Storage entities
    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
    
    // Navigation property for the related Employee entities
    public virtual ICollection<Employee> Employees { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
    
    public virtual ICollection<Item> Items { get; set; }
    
    public virtual ICollection<Discount> Discounts { get; set; }
}