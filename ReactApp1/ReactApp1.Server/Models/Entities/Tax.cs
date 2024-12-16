using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReactApp1.Server.Models;

[Table("Tax")]
public class Tax
{
    public Tax()
    {
        ServiceTax = new HashSet<ServiceTax>();
        ItemTax = new HashSet<ItemTax>();
    }

    [Key]
    public int TaxId { get; set; }
    [Column("Percentage")]
    public decimal Percentage { get; set; }

    [Column("Description")]
    public string Description { get; set; }

    public virtual ICollection<ServiceTax> ServiceTax { get; set; }
    public virtual ICollection<ItemTax> ItemTax { get; set; }

}

