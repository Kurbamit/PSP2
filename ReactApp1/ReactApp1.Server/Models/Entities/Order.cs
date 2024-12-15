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
    public decimal? DiscountPercentage { get; set; }
    
    [Column("DiscountFixed")]
    public decimal? DiscountFixed { get; set; }

    [Column("TipPercentage")]
    public decimal? TipPercentage { get; set; }

    [Column("TipFixed")]
    public decimal? TipFixed { get; set; }

    [Column("PaymentId")]
    public int? PaymentId { get; set; }
    
    [Column("Refunded")]
    public bool Refunded { get; set; }
    
    [Column("ReservationId")]
    public int? ReservationId { get; set; }
    
    [Column("EstablishmentId")]
    public int? EstablishmentId { get; set; }
    
    [Column("DiscountId")]
    public int? DiscountId { get; set; }
    
    [ForeignKey(nameof(CreatedByEmployeeId))]
    public virtual Employee CreatedByEmployee { get; set; }
    
    [ForeignKey(nameof(ReservationId))]
    public virtual Reservation Reservation { get; set; }
    
    [ForeignKey(nameof(EstablishmentId))]
    public virtual Establishment Establishment { get; set; }
    
    [ForeignKey(nameof(DiscountId))]
    public virtual Discount Discount { get; set; }
    
    public virtual ICollection<FullOrder> FullOrders { get; set; }

    public virtual ICollection<FullOrderService> FullOrderServices { get; set; }
    
    public virtual ICollection<Payment> Payments { get; set; }
}