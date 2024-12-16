using ReactApp1.Server.Migrations;

namespace ReactApp1.Server.Models.Models.Domain;

public class FullOrderServiceModel
{
    public int FullOrderServiceId { get; set; }
    public int OrderId { get; set; }
    public int ServiceId { get; set; }
    public int Count { get; set; }
    public int? DiscountId { get; set; }

    public FullOrderServiceModel()
    {
    }

    public FullOrderServiceModel(int fullOrderServiceId, int order, int serviceId, int count, int? discountId)
    {
        FullOrderServiceId = fullOrderServiceId;
        OrderId = order;
        ServiceId = serviceId;
        Count = count;
        DiscountId = discountId;
    }

    public FullOrderServiceModel(FullOrderService fullOrderService)
        : this(fullOrderService.FullOrderId, fullOrderService.OrderId, fullOrderService.ServiceId, fullOrderService.Count, fullOrderService.DiscountId)
    {

    }
}