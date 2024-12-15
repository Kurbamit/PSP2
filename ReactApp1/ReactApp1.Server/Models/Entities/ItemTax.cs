using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ReactApp1.Server.Models;

[Table("ItemTax")]
public class ItemTax
{
    public ItemTax()
    {
        
    }

    [Key]
    public int ItemTaxId { get; set; }
    [Column("ItemId")]
    public int ItemId { get; set; }

    [Column("TaxId")]
    public int TaxId { get; set; }

    [ForeignKey(nameof(ItemId))]
    public virtual Item Item { get; set; }

    [ForeignKey(nameof(TaxId))]
    public virtual Tax Tax { get; set; }

}
