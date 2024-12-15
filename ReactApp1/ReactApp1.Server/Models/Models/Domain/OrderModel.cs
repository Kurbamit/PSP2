namespace ReactApp1.Server.Models.Models.Domain;

public class OrderModel
{
    public int OrderId { get; set; }
    public int Status { get; set; }
    public int CreatedByEmployeeId { get; set; }
    public DateTime ReceiveTime { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? DiscountFixed { get; set; }
    public decimal? TipPercentage { get; set; }
    public decimal? TipFixed { get; set; }
    public int? PaymentId { get; set; }
    public bool Refunded { get; set; }
    public int? ReservationId { get; set; }
    public string? CreatedByEmployeeName { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? TotalPaid { get; set; }
    public decimal? LeftToPay { get; set; }
    public decimal? TipAmount { get; set; }
    public int? DiscountId { get; set; }
    public string? DiscountName { get; set; }

    public OrderModel()
    {
    }
    
    public OrderModel(int orderId, int status, int createdByEmployeeId, DateTime receiveTime, 
        decimal? discountPercentage, decimal? discountFixed, decimal? tipPercentage, decimal? tipFixed, int? paymentId, 
        bool refunded, int? reservationId, int? discountId)
    {
        OrderId = orderId;
        Status = status;
        CreatedByEmployeeId = createdByEmployeeId;
        ReceiveTime = receiveTime;
        DiscountPercentage = discountPercentage;
        DiscountFixed = discountFixed;
        TipPercentage = tipPercentage;
        TipFixed = tipFixed;
        PaymentId = paymentId;
        Refunded = refunded;
        ReservationId = reservationId;
        DiscountId = discountId;
    }
    
    public OrderModel(Order order)
    : this(order.OrderId, order.Status, order.CreatedByEmployeeId, order.ReceiveTime, 
        order.DiscountPercentage, order.DiscountFixed, order.TipPercentage, order.TipFixed, order.PaymentId, 
        order.Refunded, order.ReservationId, order.DiscountId)
    {
        
    }
}

public class OrderItemsPayments
{
    public OrderModel Order { get; set; }
    public List<ItemModel> Items { get; set; }
    public List<ServiceModel> Services { get; set; }
    public List<PaymentModel> Payments { get; set; }

    public OrderItemsPayments(OrderModel? order, List<ItemModel>? items, List<ServiceModel>? services, List<PaymentModel> payments)
    {
        Order = order ?? new OrderModel();
        Items = items ?? new List<ItemModel>();
        Services = services ?? new List<ServiceModel>();
        Payments = payments ?? new List<PaymentModel>();
    }
}