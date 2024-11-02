using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Employee")]
public class Employee
{
    public Employee()
    {
        Orders = new HashSet<Order>();
    }
    
    [Key]
    [Column("EmployeeId")]
    public int EmployeeId { get; set; }

    [Column("Title")]
    public int Title { get; set; }

    [Column("EstablishmentId")]
    public int EstablishmentId { get; set; }

    [Column("AddressId")]
    public int AddressId { get; set; }

    [Column("PersonalCode")]
    [StringLength(255)]
    public string? PersonalCode { get; set; }
    
    [Column("FirstName")]
    [StringLength(255)]
    public string? FirstName { get; set; }
    
    [Column("LastName")]
    [StringLength(255)]
    public string? LastName { get; set; }

    [Column("BirthDate")]
    public DateTime? BirthDate { get; set; }

    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("Phone")]
    [StringLength(255)]
    public string? Phone { get; set; }
    
    [Column("Email")]
    [StringLength(255)]
    public string? Email { get; set; }
    
    [Column("PasswordHash")]
    [StringLength(255)]
    public string? PasswordHash { get; set; }
    
    [ForeignKey(nameof(AddressId))]
    public virtual EmployeeAddress EmployeeAddress { get; set; }
    
    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment Establishment { get; set; }
    
    public ICollection<Order> Orders { get; set; }
}