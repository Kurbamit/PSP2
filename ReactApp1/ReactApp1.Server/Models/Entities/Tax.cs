using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReactApp1.Server.Models;

[Table("Tax")]
public class Tax
{
    public Tax()
    {
            
    }

    [Key]
    public int TaxId { get; set; }
    [Column("Percentage")]
    public decimal Percentage { get; set; }

    [Column("Description")]
    public string Description { get; set; }


}

