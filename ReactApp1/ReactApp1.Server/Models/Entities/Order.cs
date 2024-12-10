using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models;

[Table("Order")]
public class Order
{
    public Order()
    {
        FullOrders = new HashSet<FullOrder>();
        Payments = new HashSet<Payment>();
    }
    
    [Key]
    [Column("OrderId")]
    public int OrderId { get; set; }

    [Column("Status")]
    public int Status { get; set; }

    [Column("CreatedByEmployeeId")]
    public int CreatedByEmployeeId { get; set; }
    
    [Column("ReceiveTime")]
    public DateTime ReceiveTime { get; set; }

    [Column("DiscountPercentage")]
    public int? DiscountPercentage { get; set; }
    
    [Column("DiscountFixed")]
    public decimal? DiscountFixed { get; set; }

    [Column("TipPercentage")]
    public int? TipPercentage { get; set; }

    [Column("TipFixed")]
    public decimal? TipFixed { get; set; }

    [Column("PaymentId")]
    public int? PaymentId { get; set; }
    
    [Column("Refunded")]
    public bool Refunded { get; set; }
    
    [Column("ReservationId")]
    public int? ReservationId { get; set; }
    
    [ForeignKey(nameof(CreatedByEmployeeId))]
    public virtual Employee CreatedByEmployee { get; set; }
    
    [ForeignKey(nameof(ReservationId))]
    public virtual Reservation Reservation { get; set; }
    
    public virtual ICollection<FullOrder> FullOrders { get; set; }
    
    public virtual ICollection<Payment> Payments { get; set; }
}