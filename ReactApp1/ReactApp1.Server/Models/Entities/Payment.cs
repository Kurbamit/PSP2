using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Payment")]
public class Payment
{
    public Payment()
    {

    }
    
    [Key]
    [Column("PaymentId")]
    public int PaymentId { get; set; }
    
    [Column("OrderId")]
    public int OrderId { get; set; }
    
    [Column("Type")]
    public int Type { get; set; }

    [Column("Amount")]
    public decimal Value { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }
    
    [Column("GiftCardId")]
    public int GiftCardId { get; set; }

    [Column("StripePaymentId")]
    public string StripePaymentId { get; set; }
    
    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; }
}