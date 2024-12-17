using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReactApp1.Server.Models;

[Table("FullOrderServiceTax")]
public class FullOrderServiceTax
{
    public FullOrderServiceTax()
    {
        
    }

    [Key]
    [Column("FullOrderServiceTaxId")]
    public int FullOrderServiceTaxId { get; set; }

    [Column("FullOrderId")]
    public int FullOrderServiceId { get; set; }

    [Column("Percentage")]
    public decimal Percentage { get; set; }

    [Column("Description")]
    public string Description { get; set; }


}
