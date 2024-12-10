namespace ReactApp1.Server.Models.Models.Domain;

public class ReservationModel
{
    /// <summary>
    /// Identifikatorius
    /// </summary>
    public int ReservationID { get; set; }

    /// <summary>
    /// Kada gauta rezervacija
    /// </summary>
    public DateTime ReceiveTime { get; set; }

    /// <summary>
    /// Rezervacijos pradzios laikas
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Rezervacijos pabaigos laikas
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Uzsakovu kiekis
    /// </summary>
    public int CustomerCount { get; set; }

    /// <summary>
    /// Uzrezervuota vieta
    /// </summary>
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