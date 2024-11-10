namespace ReactApp1.Server.Models.Models.Domain;

public class ReservationModel
{
    public int ReservationID { get; set; }
    public DateTime ReceiveTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int CustomerCount { get; set; }
    public string? ReservedSpot { get; set; }

    public void MapUpdate(Reservation existingReservation)
    {
        existingReservation.ReceiveTime = ReceiveTime;
        existingReservation.StartTime = StartTime;
        existingReservation.EndTime = EndTime;
        existingReservation.CustomerCount = CustomerCount;
        existingReservation.ReservedSpot = ReservedSpot;
    }
}