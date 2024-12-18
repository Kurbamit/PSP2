using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models.Models.Domain;

public class PaymentModel
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public int Type { get; set; }

    public decimal Value { get; set; }

    public DateTime ReceiveTime { get; set; }

    public int GiftCardId { get; set; }

    public string GiftCardCode { get; set; }

    public string StripePaymentId { get; set; }

    public void MapUpdate(Payment existingPayment)
    {
        existingPayment.OrderId = this.OrderId;
        existingPayment.Type = this.Type;
        existingPayment.Value = this.Value;
        existingPayment.ReceiveTime = this.ReceiveTime;
        existingPayment.GiftCardId = this.GiftCardId;
        existingPayment.StripePaymentId = this.StripePaymentId;
    }

}