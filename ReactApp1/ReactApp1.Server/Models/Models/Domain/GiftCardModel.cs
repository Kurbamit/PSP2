namespace ReactApp1.Server.Models.Models.Domain;

public class GiftCardModel
{
    public int GiftCardId { get; set; }

    public DateTime ExpirationDate { get; set; }

    public decimal Amount { get; set; }
        
    public string Code { get; set; }

    public DateTime ReceiveTime { get; set; }


    public void MapUpdate(GiftCard existingGiftCard)
    {
        existingGiftCard.ExpirationDate = this.ExpirationDate;
        existingGiftCard.Amount = this.Amount;
        existingGiftCard.Code = this.Code;
        existingGiftCard.ReceiveTime = this.ReceiveTime;
    }
}