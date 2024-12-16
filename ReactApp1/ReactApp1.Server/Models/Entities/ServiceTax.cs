using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReactApp1.Server.Models;

[Table("ServiceTax")]
public class ServiceTax
{
    public ServiceTax()
    {
        
    }

    [Column("ServiceId")]
    public int ServiceId { get; set; }

    [Column("TaxId")]
    public int TaxId { get; set; }

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; }

    [ForeignKey(nameof(TaxId))]
    public virtual Tax Tax { get; set; }

}
