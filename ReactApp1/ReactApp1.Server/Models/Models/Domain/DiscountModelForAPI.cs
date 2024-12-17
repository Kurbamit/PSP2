namespace ReactApp1.Server.Models.Models.Domain;

public class DiscountModelForAPI
{
    public int DiscountId { get; set; }
    public string DiscountName { get; set; }
    public decimal Value { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int? EstablishmentId { get; set; }
}