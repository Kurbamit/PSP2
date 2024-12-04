namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderModel
{
    public int FullOrderId { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Count { get; set; }
    
    
    public FullOrderModel(int fullOrderId, int order, int itemId, int count)
    {
        FullOrderId = fullOrderId;
        OrderId = order;
        ItemId = itemId;
        Count = count;
    }
    
    public FullOrderModel(FullOrder fullOrder)
        : this(fullOrder.FullOrderId, fullOrder.OrderId, fullOrder.ItemId, fullOrder.Count)
    {
        
    }
}