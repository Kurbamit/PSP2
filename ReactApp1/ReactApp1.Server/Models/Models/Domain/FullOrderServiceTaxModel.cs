using Stripe.Climate;
using Stripe;

namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderServiceTaxModel
{
    public FullOrderServiceTaxModel()
    {
        
    }

    public int FullOrderServiceTaxId { get; set; }

    public int FullOrderServiceId { get; set; }

    public decimal Percentage { get; set; }

    public string Description { get; set; }

    public void MapUpdate(FullOrderServiceTax existingModel)
    {
        existingModel.FullOrderServiceId = this.FullOrderServiceId;
        existingModel.Percentage = this.Percentage;
        existingModel.Description = this.Description;

    }

}
