using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.Models.Models.Domain;

public class ItemModel
{
    /// <summary>
    /// Identifikatorius
    /// </summary>
    public int ItemId { get; set; }
    
    /// <summary>
    /// Pavadinimas
    /// </summary>
    [StringLength(255)]
    public string? Name { get; set; }
    
    /// <summary>
    /// Kaina
    /// </summary>
    public decimal? Cost { get; set; }
    
    /// <summary>
    /// Mokesčiai
    /// </summary>
    public decimal? Tax { get; set; }
    
    /// <summary>
    /// Ar alkoholinis gėrimas
    /// </summary>
    public bool AlcoholicBeverage { get; set; }
    
    public DateTime ReceiveTime { get; set; }

    public  int? Storage { get; set; }
    
    public int? Count { get; set; }
    
    public decimal? Discount { get; set; }

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