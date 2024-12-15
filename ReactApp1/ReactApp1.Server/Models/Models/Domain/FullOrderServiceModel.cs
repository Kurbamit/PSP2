namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderServiceModel
{
    public int OrderId { get; set; }
    public int ServiceId { get; set; }
    public int Count { get; set; }

    public FullOrderServiceModel()
    {
    }

    public FullOrderServiceModel(int order, int serviceId, int count)
    {
        OrderId = order;
        ServiceId = serviceId;
        Count = count;
    }

    public FullOrderServiceModel(FullOrderService fullOrderService)
        : this(fullOrderService.OrderId, fullOrderService.ServiceId, fullOrderService.Count)
    {

    }
}