using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("GiftCard")]
public class GiftCard
{
    [Key]
    [Column("GiftCardId")]
    public int GiftCardId { get; set; }
    
    [Column("ExpirationDate")]
    public DateTime ExpirationDate { get; set; }
    
    [Column("Amount")]
    public decimal Amount { get; set; }
    
    [Column("Code")]
    [StringLength(255)]
    public string Code { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("PaymentId")]
    public int? PaymentId { get; set; }
    
    [ForeignKey(nameof(PaymentId))]
    public virtual Payment Payment { get; set; }
}