namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderModel
{
    public int FullOrderId { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Count { get; set; }
    public int? DiscountId { get; set; }
    
    public FullOrderModel()
    {
    }
    
    public FullOrderModel(int fullOrderId, int order, int itemId, int count, int? discountId)
    {
        FullOrderId = fullOrderId;
        OrderId = order;
        ItemId = itemId;
        Count = count;
        DiscountId = discountId;
    }
    
    public FullOrderModel(FullOrder fullOrder)
        : this(fullOrder.FullOrderId, fullOrder.OrderId, fullOrder.ItemId, fullOrder.Count, fullOrder.DiscountId)
    {
        
    }
}