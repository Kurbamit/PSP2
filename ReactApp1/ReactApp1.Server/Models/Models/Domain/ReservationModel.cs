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
    /// Identifikatorius darbuotojo, kuris sukure rezervacija
    /// </summary>
    public int CreatedByEmployeeId { get; set; }

    /// <summary>
    /// Identifikatorius istaigos, kuriame siuloma rezervacija
    /// </summary>
    public int EstablishmentId { get; set; }

    /// <summary>
    /// Identifikatorius istaigos adreso, kuriame vyks rezervacija
    /// </summary>
    public int EstablishmentAddressId { get; set; }

    /// <summary>
    /// Identifikatorius serviso, kuris bus atliekamas rezervacijoje
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// Kliento telefono numeris
    /// </summary>
    public string CustomerPhoneNumber { get; set; }

    public void MapUpdate(Reservation existingReservation)
    {
        existingReservation.ReceiveTime = ReceiveTime;
        existingReservation.StartTime = StartTime;
        existingReservation.EndTime = EndTime;
        existingReservation.CreatedByEmployeeId = CreatedByEmployeeId;
        existingReservation.EstablishmentId = EstablishmentId;
        existingReservation.EstablishmentAddressId = EstablishmentAddressId;
        existingReservation.ServiceId = ServiceId;
        existingReservation.CustomerPhoneNumber = CustomerPhoneNumber;
    }
}