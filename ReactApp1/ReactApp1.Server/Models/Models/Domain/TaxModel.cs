namespace ReactApp1.Server.Models.Models.Domain;

public class TaxModel
{
    public TaxModel()
    {
        
    }

    public int TaxId { get; set; }
    public decimal Percentage { get; set; }
    public string Description { get; set; }

}
