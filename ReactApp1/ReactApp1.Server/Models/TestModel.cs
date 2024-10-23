using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReactApp1.Server.Models;

[Table("Test")]
public class TestModel
{
    [Key]
    [Column("GuidId")]
    public Guid GuidId { get; set; }
    
    [Column("IntId")]
    public int IntId { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
}