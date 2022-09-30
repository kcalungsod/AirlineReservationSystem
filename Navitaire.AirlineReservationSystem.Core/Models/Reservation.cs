namespace Navitaire.AirlineReservationSystem.Core.Models
{
    public class ReservationList
    {
        public Reservation[]? ReservationDB;
    }
    public class Reservation
    {
        private string? _PNRNumber;

        public Flight? FlightDetails { get; set; }
        public DateTime? FlightDate { get; set; }
        public int NumberOfPassengers { get; set; }
        public List<Passenger>? ListOfPassengers { get; set; }
        public string? PNRNumber
        {
            get => _PNRNumber;
            set => _PNRNumber = value?.ToUpper();
        }
    }
}
