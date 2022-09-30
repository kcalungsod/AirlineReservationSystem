using Navitaire.AirlineReservationSystem.Core.Models;

namespace Navitaire.AirlineReservationSystem.Core.Common
{
    public class TestDB
    {
        public static List<Flight> CurrentFlights { get; set; } = new();
        public static List<Reservation> CurrentReservations { get; set; } = new();

        public static void Addflight(Flight newFlight)
        {
            CurrentFlights?.Add(newFlight);
        }

        public static void AddReservation(Reservation newReservation)
        {
            CurrentReservations?.Add(newReservation);
        }
    }
}