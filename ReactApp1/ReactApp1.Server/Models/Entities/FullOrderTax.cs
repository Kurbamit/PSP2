using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReactApp1.Server.Models;

[Table("FullOrderTax")]
public class FullOrderTax
{
    public FullOrderTax()
    {
        
    }

    [Key]
    [Column("FullOrderTaxId")]
    public int FullOrderTaxId { get; set; }

    [Column("FullOrderId")]
    public int FullOrderId { get; set; }

    [Column("Percentage")]
    public decimal Percentage { get; set; }

    [Column("Description")]
    public string Description { get; set; }

}
