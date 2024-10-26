using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("EmployeeAddress")]
public class EmployeeAddress
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
    
    [Column("HouseNumber")]
    [StringLength(255)]
    public string? HouseNumber { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("EmployeeId")] 
    public int EmployeeId { get; set; }
    
    public virtual Employee Employee { get; set; }
}