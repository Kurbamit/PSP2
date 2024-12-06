namespace ReactApp1.Server.Models.Models.Domain;

public class OrderModel
{
    public int OrderId { get; set; }
    public int Status { get; set; }
    public int CreatedByEmployeeId { get; set; }
    public DateTime ReceiveTime { get; set; }
    public int? DiscountPercentage { get; set; }
    public decimal? DiscountFixed { get; set; }
    public int? PaymentId { get; set; }
    public bool Refunded { get; set; }
    public int? ReservationId { get; set; }

    public OrderModel()
    {
    }
    
    public OrderModel(int orderId, int status, int createdByEmployeeId, DateTime receiveTime, 
        int? discountPercentage, decimal? discountFixed, int? paymentId, bool refunded, int? reservationId)
    {
        OrderId = orderId;
        Status = status;
        CreatedByEmployeeId = createdByEmployeeId;
        ReceiveTime = receiveTime;
        DiscountPercentage = discountPercentage;
        DiscountFixed = discountFixed;
        PaymentId = paymentId;
        Refunded = refunded;
        ReservationId = reservationId;
    }
    
    public OrderModel(Order order)
    : this(order.OrderId, order.Status, order.CreatedByEmployeeId, order.ReceiveTime, 
        order.DiscountPercentage, order.DiscountFixed, order.PaymentId, order.Refunded, order.ReservationId)
    {
        
    }
}

public class OrderItems
{
    public OrderModel Order { get; set; }
    public List<ItemModel> Items { get; set; }

    public OrderItems(OrderModel? order, List<ItemModel>? items)
    {
        Order = order ?? new OrderModel();
        Items = items ?? new List<ItemModel>();
    }
}