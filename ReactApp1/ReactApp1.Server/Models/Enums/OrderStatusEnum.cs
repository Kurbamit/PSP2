namespace ReactApp1.Server.Models.Enums;

public enum OrderStatusEnum
{
    None = 0,
    Open = 1, // Can add items, can't pay
    Closed = 2, // Can't add items, can pay, waiting for all payments
    Cancelled = 3,
    Completed = 4 // Order fully paid
}