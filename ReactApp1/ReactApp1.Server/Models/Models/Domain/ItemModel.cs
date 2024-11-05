namespace ReactApp1.Server.Models.Models.Domain;

public class ItemModel
{
    public int ItemId { get; set; }
    
    public string? Name { get; set; }
    
    public decimal? Cost { get; set; }
    
    public decimal? Tax { get; set; }
    
    public bool AlcoholicBeverage { get; set; }
    
    public DateTime ReceiveTime { get; set; }

    public  int? Storage { get; set; }

    public void MapUpdate(Item existingItem)
    {
        existingItem.Name = this.Name;
        existingItem.Cost = this.Cost;
        existingItem.Tax = this.Tax;
        existingItem.AlcoholicBeverage = this.AlcoholicBeverage;
        existingItem.ReceiveTime = this.ReceiveTime;
        if (existingItem.Storage != null && this.Storage.HasValue)
        {
            existingItem.Storage.Count = this.Storage.Value;
        }
    }
}