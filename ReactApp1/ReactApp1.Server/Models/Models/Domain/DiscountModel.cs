namespace ReactApp1.Server.Models.Models.Domain
{
    public class DiscountModel
    {
        public int OrderId { get; set; }
        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
        public decimal Value { get; set; }
        
        // If discount applied to a specific item in the order
        public int? ItemId { get; set; }
    }
}

