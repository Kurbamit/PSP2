namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderTaxModel
{
    public FullOrderTaxModel()
    {
        
    }

    public int FullOrderTaxId { get; set; }

    public int FullOrderId { get; set; }

    public decimal Percentage { get; set; }

    public string Description { get; set; }

    public void MapUpdate(FullOrderTax existingModel)
    {
        existingModel.FullOrderId = this.FullOrderId;
        existingModel.Percentage = this.Percentage;
        existingModel.Description = this.Description;

    }

}
