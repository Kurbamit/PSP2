namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderModel
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Count { get; set; }
    
    public FullOrderModel()
    {
    }
    
    public FullOrderModel(int order, int itemId, int count)
    {
        OrderId = order;
        ItemId = itemId;
        Count = count;
    }
    
    public FullOrderModel(FullOrder fullOrder)
        : this(fullOrder.OrderId, fullOrder.ItemId, fullOrder.Count)
    {
        
    }
}