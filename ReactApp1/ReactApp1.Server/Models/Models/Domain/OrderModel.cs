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

    public OrderModel()
    {
    }
    
    public OrderModel(int orderId, int status, int createdByEmployeeId, DateTime receiveTime, 
        decimal? discountPercentage, decimal? discountFixed, decimal? tipPercentage, decimal? tipFixed, int? paymentId, 
        bool refunded, int? reservationId)
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
    }
    
    public OrderModel(Order order)
    : this(order.OrderId, order.Status, order.CreatedByEmployeeId, order.ReceiveTime, 
        order.DiscountPercentage, order.DiscountFixed, order.TipPercentage, order.TipFixed, order.PaymentId, 
        order.Refunded, order.ReservationId)
    {
        
    }
}

public class OrderItemsPayments
{
    public OrderModel Order { get; set; }
    public List<ItemModel> Items { get; set; }
    public List<PaymentModel> Payments { get; set; }

    public OrderItemsPayments(OrderModel? order, List<ItemModel>? items, List<PaymentModel> payments)
    {
        Order = order ?? new OrderModel();
        Items = items ?? new List<ItemModel>();
        Payments = payments ?? new List<PaymentModel>();
    }
}